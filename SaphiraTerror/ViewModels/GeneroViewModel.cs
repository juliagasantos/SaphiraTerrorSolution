using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Models;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.ViewModels
{
    public class GeneroViewModel
    {
        public int IdGenero { get; set; }
        public string DescricaoGenero { get; set; }

        public IEnumerable<SelectListItem>? Filmes { get; set; }

    }
}
