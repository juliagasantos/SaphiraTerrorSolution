using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.Repositories;
using SaphiraTerror.ViewModels;

namespace SaphiraTerror.Controllers
{
    public class FilmeController : Controller
    {
        //campo de apoio
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;

        //injesão de dependência
        public FilmeController(IFilmeRepository filmeRepository, IGeneroRepository generoRepository, IClassificacaoRepository classificacaoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _classificacaoRepository = classificacaoRepository;
        }

        //create filme viewmodel
        private async Task<FilmeViewModel> CriarFilmeViewModel(FilmeViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();
            var classificacoes = await _classificacaoRepository.GetAllAsync();

            return new FilmeViewModel
            {
                IdFilmeViewModel = model?.IdFilmeViewModel ?? 0,
                TituloFilmeViewModel = model?.TituloFilmeViewModel,
                ProdutoraFilmeViewModel = model?.ProdutoraFilmeViewModel,
                GeneroIdFilmeModel = model?.GeneroIdFilmeModel ?? 0,
                ClassificacaoIdFilmeViewModel = model?.ClassificacaoIdFilmeViewModel ?? 0,
                UrlImagemFilmeViewModel = model?.UrlImagemFilmeViewModel,
                ImagemUpload = model?.ImagemUpload,
                Generos = generos.Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero.ToString()
                }),
                Classificacoes = classificacoes.Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao.ToString()
                })
            };
        }

        //refator em seguida
        public async Task<IActionResult> Index(int? generoId, string? search)
        {
            var filmes = await _filmeRepository.GetAllAsync();
            //filtro por genero
            if (generoId.HasValue && generoId.Value > 0)
            {
                filmes = filmes.Where(f => f.GeneroId == generoId).ToList();
            }
            //busca por nome
            if (!string.IsNullOrEmpty(search))
            {
                filmes = filmes.Where(f => f.Titulo.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            filmes = filmes.OrderByDescending(f => f.IdFilme).ToList();
            ViewBag.Generos = new SelectList(await _generoRepository.GetAllAsync(), "IdGenero", "DescricaoGenero");
            ViewBag.FiltroGeneroId = generoId;
            ViewBag.Search = search;

            return View(filmes);
        }

        //create
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarFilmeViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string caminhoImagem = null;
                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);
                    var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);
                    caminhoImagem = "/img/" + nomeArquivo;
                }
                var filme = new Filme
                {
                    Titulo = viewModel.TituloFilmeViewModel,
                    Produtora = viewModel.ProdutoraFilmeViewModel,
                    GeneroId = viewModel.GeneroIdFilmeModel,
                    ClassificacaoId = viewModel.ClassificacaoIdFilmeViewModel,
                    UrlImagem = caminhoImagem
                };
                await _filmeRepository.AddAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);
        }


        //edit
        public async Task<IActionResult> Edit(int id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null) return NotFound();

            var viewModel = new FilmeViewModel
            {
                IdFilmeViewModel = filme.IdFilme,
                TituloFilmeViewModel = filme.Titulo,
                ProdutoraFilmeViewModel = filme.Produtora,
                UrlImagemFilmeViewModel = filme.UrlImagem,
                GeneroIdFilmeModel = filme.GeneroId,
                ClassificacaoIdFilmeViewModel = filme.ClassificacaoId,
                Generos = (await _generoRepository.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero

                }),
                Classificacoes = (await _classificacaoRepository.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.IdClassificacao.ToString(),
                    Text = g.DescricaoClassificacao
                })
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id, FilmeViewModel viewModel)
        {
            if (id != viewModel.IdFilmeViewModel) return NotFound();
            if (ModelState.IsValid)
            {
                var filme = await _filmeRepository.GetByIdAsync(id);
                if (filme == null) return NotFound();

                filme.Titulo = viewModel.TituloFilmeViewModel;
                filme.Produtora = viewModel.ProdutoraFilmeViewModel;
                filme.GeneroId = viewModel.GeneroIdFilmeModel;
                filme.ClassificacaoId = viewModel.ClassificacaoIdFilmeViewModel;


                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);
                    var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);
                    filme.UrlImagem = "/img/" + nomeArquivo;
                }
                await _filmeRepository.UpdateAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null) return NotFound();

            return View(filme);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _filmeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
