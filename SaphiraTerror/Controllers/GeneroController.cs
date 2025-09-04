using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.Repositories;
using SaphiraTerror.ViewModels;

namespace Salvation.Controllers
{
    public class GeneroController : Controller
    {
        
        //campo de apoio para injeção de dependência
        private readonly IGeneroRepository _generoRepository;



        //construtor
        public GeneroController(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }
        //Classificacao
        //classificacao

        //metodo de apoio criar filmeVM
        private async Task<GeneroViewModel> CriarGeneroViewModel(GeneroViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();

            return new GeneroViewModel
            {
                IdGenero = model?.IdGenero ?? 0,
                DescricaoGenero = model?.DescricaoGenero,
                Filmes = generos.Select(c => new SelectListItem
                {
                    Value = c.IdGenero.ToString(),
                    Text = c.DescricaoGenero
                })
            };
        }

        //idex
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index(int? generoId, string? search)
        {
            var generos = await _generoRepository.GetAllAsync();

            //ordem decrescente
            generos = generos.OrderByDescending(f => f.IdGenero).ToList();

            return View(generos);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarGeneroViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GeneroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = new Genero
                {
                    DescricaoGenero = viewModel.DescricaoGenero
                };
                await _generoRepository.AddAsync(genero);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel(viewModel);
            return View(viewModel);
        }

        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null) return NotFound();

            var viewModel = new GeneroViewModel
            {
                IdGenero = genero.IdGenero,
                DescricaoGenero = genero.DescricaoGenero

            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GeneroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = await _generoRepository.GetByIdAsync(id);
                if (genero == null) return NotFound();

                genero.IdGenero = viewModel.IdGenero;
                genero.DescricaoGenero= viewModel.DescricaoGenero;

                await _generoRepository.UpdateAsync(genero);

                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null) return NotFound();
            return View(genero);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _generoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

