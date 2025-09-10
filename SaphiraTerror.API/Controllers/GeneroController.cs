using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaphiraTerror.API.DTOs;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly IGeneroRepository _generoRepository;
        public GeneroController(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //buscar todos os generos
            var generos = await _generoRepository.GetAllAsync();
            //criar lista de resultados
            var resultado = new List<GeneroOutputDTO>();
            foreach (var genero in generos)
            {
               resultado.Add(new GeneroOutputDTO {
                   IdGenero = genero.IdGenero,
                   Descricao = genero.DescricaoGenero
               });
            }
            return Ok(resultado);
        }
    }
}
