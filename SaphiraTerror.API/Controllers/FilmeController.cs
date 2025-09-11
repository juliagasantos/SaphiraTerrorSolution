using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaphiraTerror.API.DTOs;
using SaphiraTerror.Interfaces;

namespace SaphiraTerror.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        private readonly IFilmeRepository _filmeRepository;
        public FilmeController(IFilmeRepository filmeRepository)
        {
            _filmeRepository = filmeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var filmes = await _filmeRepository.GetAllAsync();
            var resultado = new List<FilmeOutputDTO>();
            foreach (var filme in filmes)
            {
                resultado.Add(new FilmeOutputDTO
                {
                    IdFilme = filme.IdFilme,
                    Titulo = filme.Titulo,
                    Produtora = filme.Produtora,
                    UrlImagem = filme.UrlImagem,
                    Genero = filme.Genero?.DescricaoGenero,
                    Classificacao = filme.Classificacao?.DescricaoClassificacao
                });
            }
            return Ok(resultado);
        }

        [HttpGet(":idGenero")]
        public async Task<ActionResult> Get(int idGenero)
        {
            var filmes = await _filmeRepository.GetByIdGeneroAsync(idGenero);
            var resultado = new List<FilmeOutputDTO>();
            foreach (var filme in filmes)
            {
                resultado.Add(new FilmeOutputDTO
                {
                    IdFilme = filme.IdFilme,
                    Titulo = filme.Titulo,
                    Produtora = filme.Produtora,
                    UrlImagem = filme.UrlImagem,
                    Genero = filme.Genero?.DescricaoGenero,
                    Classificacao = filme.Classificacao?.DescricaoClassificacao
                });
            }
            return Ok(resultado);
        }
    }
}
