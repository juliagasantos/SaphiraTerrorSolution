using SaphiraTerror.Models;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.API.DTOs
{
    public class FilmeOutputDTO
    {
        public int IdFilme { get; set; } 
        public string? Titulo { get; set; }
        public string? Produtora { get; set; }
        public string? UrlImagem { get; set; }
        public string? Genero { get; set; }

        public string? Classificacao { get; set; }
    }
}
