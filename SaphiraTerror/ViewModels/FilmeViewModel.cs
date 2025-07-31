using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace SaphiraTerror.ViewModels
{
    public class FilmeViewModel
    {
        public int IdFilmeViewModel { get; set; }
        public string TituloFilmeViewModel { get; set; }
        public string ProdutoraFilmeViewModel { get; set; }
        public string UrlImagemFilmeViewModel { get; set; }

        public IFormFile? ImagemUpload { get; set; }

        public int ClassificacaoIdFilmeViewModel { get; set; }
        public int GeneroIdFilmeModel { get; set; }

        public IEnumerable<SelectListItem>? Classificacoes { get; set; }
        public IEnumerable<SelectListItem>? Generos { get; set; }
    }
}
