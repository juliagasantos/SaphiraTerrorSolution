using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.ViewModels;
using System.Security.Claims;

namespace SaphiraTerror.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository, ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        //index
        [Authorize(Roles = "Administrador,Gerente,Outros")]
        public async Task<IActionResult> Index(int? tipoUsuarioId, string? search)

        {
            var usuarios = await _usuarioRepository.GetAllAtivosAsync();
            if (tipoUsuarioId.HasValue && tipoUsuarioId.Value > 0)
                usuarios = usuarios.Where(u => u.TipoUsuarioId == tipoUsuarioId).ToList();

            if (!string.IsNullOrEmpty(search))
                usuarios = usuarios.Where(u => u.Nome.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            usuarios = usuarios.OrderByDescending(u => u.Nome).ToList();

            ViewBag.TipoUsuario = new SelectList(await _tipoUsuarioRepository.GetAllAsync(), "IdTipoUsuario", "DescricaoTipoUsuario");
            ViewBag.FiltroTipoId = tipoUsuarioId;
            ViewBag.Search = search;
            return View(usuarios);

        }

        //login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = await _usuarioRepository.ValidarLoginAsync(email, senha);
            if (usuario == null || !usuario.Ativo)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos!");
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.DescricaoTipoUsuario)
            };
            var identity = new ClaimsIdentity(claims, "SaphiraAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("SaphiraAuth", principal);

            return RedirectToAction("Index", "Home");
        }
        //logout
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("SaphiraAuth");
            return RedirectToAction("Login");
        }

        //acesso negado
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AcessoNegado()
        {
            return View();
        }

        //create
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarUsuarioViewModel();

            return View(viewModel);
        }

        //criar usuario view model
        private async Task<UsuarioViewModel> CriarUsuarioViewModel(UsuarioViewModel? model = null)
        {
            var tipos = await _tipoUsuarioRepository.GetAllAsync();
            return new UsuarioViewModel
            { 
                IdUsuario = model?.IdUsuario ?? 0,
                Nome = model?.Nome,
                Email = model?.Email,
                Senha = model?.Senha,
                DataNascimento = model?.DataNascimento ?? DateTime.Now,
                TipoUsuarioId = model?.TipoUsuarioId ?? 0,
                TiposUsuario = tipos.Select(t => new SelectListItem
                {
                    Value = t.IdTipoUsuario.ToString(),
                    Text = t.DescricaoTipoUsuario
                })
            };
        }  
    }
}
