using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Models;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.ViewModels
{
    public class TipoUsuarioViewModel
    {
        public int IdTipoUsuario { get; set; }
        public string DescricaoTipoUsuario { get; set; }

        public IEnumerable<SelectListItem>? TipoUsuarios { get; set; }

    }
}
