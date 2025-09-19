using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.API.DTOs
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Nome do remetente é obrigatório")]
        public string NomeRemetente { get; set; }
        [Required(ErrorMessage = "Email do remetente é obrigatório")]
        public string EmailRemetente { get; set; }
        public string? Telefone { get; set; }
        [Required(ErrorMessage = "Assunto é obrigatório")]
        public string Assunto { get; set; }
        [Required(ErrorMessage = "Mensagem é obrigatório")]
        public string Mensagem { get; set; }
    }
}
