using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.Repositories;
using SaphiraTerror.ViewModels;

namespace Salvation.Controllers
{
    public class TipoUsuarioController : Controller
    {
        
        //campo de apoio para injeção de dependência
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;



        //construtor
        public TipoUsuarioController(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }
        //Classificacao
        //classificacao

        //metodo de apoio criar filmeVM
        private async Task<TipoUsuarioViewModel> CriarTipoUsuarioViewModel(TipoUsuarioViewModel? model = null)
        {
            var tipoUsuarios = await _tipoUsuarioRepository.GetAllAsync();

            return new TipoUsuarioViewModel
            {
                IdTipoUsuario = model?.IdTipoUsuario ?? 0,
                DescricaoTipoUsuario = model?.DescricaoTipoUsuario,
                TipoUsuarios = tipoUsuarios.Select(c => new SelectListItem
                {
                    Value = c.IdTipoUsuario.ToString(),
                    Text = c.DescricaoTipoUsuario
                })
            };
        }

        //idex
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index(int? tipoUsuarioId, string? search)
        {
            var tipoUsuarios = await _tipoUsuarioRepository.GetAllAsync();

            //ordem decrescente
            tipoUsuarios = tipoUsuarios.OrderByDescending(f => f.IdTipoUsuario).ToList();

            return View(tipoUsuarios);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarTipoUsuarioViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoUsuario = new TipoUsuario
                {
                    DescricaoTipoUsuario = viewModel.DescricaoTipoUsuario
                };
                await _tipoUsuarioRepository.AddAsync(tipoUsuario);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarTipoUsuarioViewModel(viewModel);
            return View(viewModel);
        }

        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
            if (tipoUsuario == null) return NotFound();

            var viewModel = new TipoUsuarioViewModel
            {
                IdTipoUsuario = tipoUsuario.IdTipoUsuario,
                DescricaoTipoUsuario = tipoUsuario.DescricaoTipoUsuario
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
                if (tipoUsuario == null) return NotFound();

                tipoUsuario.IdTipoUsuario = viewModel.IdTipoUsuario;
                tipoUsuario.DescricaoTipoUsuario = viewModel.DescricaoTipoUsuario;

                await _tipoUsuarioRepository.UpdateAsync(tipoUsuario);

                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarTipoUsuarioViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
            if (tipoUsuario == null) return NotFound();
            return View(tipoUsuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoUsuarioRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

