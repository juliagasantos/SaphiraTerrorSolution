using SaphiraTerror.Models;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.API.DTOs
{
    public class GeneroOutputDTO
    {
        public int IdGenero { get; set; }
        public string? Descricao { get; set; }
    }
}
