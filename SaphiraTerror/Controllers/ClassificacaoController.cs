using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.Repositories;
using SaphiraTerror.ViewModels;

namespace Salvation.Controllers
{
    public class ClassificacaoController : Controller
    {
        
        //campo de apoio para injeção de dependência
        private readonly IClassificacaoRepository _classificacaoRepository;



        //construtor
        public ClassificacaoController(IClassificacaoRepository classificacaoRepository)
        {
            _classificacaoRepository = classificacaoRepository;
        }
        //Classificacao
        //classificacao

        //metodo de apoio criar filmeVM
        private async Task<ClassificacaoViewModel> CriarClassificacaoViewModel(ClassificacaoViewModel? model = null)
        {
            var classificacoes = await _classificacaoRepository.GetAllAsync();

            return new ClassificacaoViewModel
            {
                IdClassificacao = model?.IdClassificacao ?? 0,
                DescricaoClassificacao = model?.DescricaoClassificacao,
                Filmes = classificacoes.Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao
                })

            };
        }

        //idex
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index(int? classificacaoId, string? search)
        {
            var classificacaoes = await _classificacaoRepository.GetAllAsync();

            //ordem decrescente
            classificacaoes = classificacaoes.OrderByDescending(f => f.IdClassificacao).ToList();

            return View(classificacaoes);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarClassificacaoViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassificacaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var classificacao = new Classificacao
                {
                    DescricaoClassificacao = viewModel.DescricaoClassificacao
                };
                await _classificacaoRepository.AddAsync(classificacao);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarClassificacaoViewModel(viewModel);
            return View(viewModel);
        }

        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var classificacao = await _classificacaoRepository.GetByIdAsync(id);
            if (classificacao == null) return NotFound();

            var viewModel = new ClassificacaoViewModel
            {
                IdClassificacao = classificacao.IdClassificacao,
                DescricaoClassificacao = classificacao.DescricaoClassificacao

            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassificacaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var classificacao = await _classificacaoRepository.GetByIdAsync(id);
                if (classificacao == null) return NotFound();

                classificacao.IdClassificacao = viewModel.IdClassificacao;
                classificacao.DescricaoClassificacao = viewModel.DescricaoClassificacao;

                await _classificacaoRepository.UpdateAsync(classificacao);

                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarClassificacaoViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var classificacao = await _classificacaoRepository.GetByIdAsync(id);
            if (classificacao == null) return NotFound();
            return View(classificacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _classificacaoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

