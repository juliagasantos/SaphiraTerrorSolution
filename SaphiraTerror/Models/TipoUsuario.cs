using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaphiraTerror.Models
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        [Key]
        public int IdTipoUsuario { get; set; }
        [Required(ErrorMessage = "Campo obrigatório!")]
        [Display(Name = "Tipo de Usuário")]
        [StringLength(150)]
        public int DescricaoTipoUsuaario { get; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
