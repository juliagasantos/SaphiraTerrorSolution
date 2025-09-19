using SaphiraTerror.API.DTOs;

namespace SaphiraTerror.API.Interfaces
{
    public interface IEmailService
    {
        public void Enviar(EmailDTO email);
    }
}
