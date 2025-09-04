using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.ViewModels
{
    public class ClassificacaoViewModel
    {
            public int IdClassificacao { get; set; }
            public string DescricaoClassificacao { get; set; }
            public IEnumerable<SelectListItem>? Filmes { get; set; }
    }
}
