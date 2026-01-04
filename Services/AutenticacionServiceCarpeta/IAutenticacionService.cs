using API_de_Reservas.DTOs.AutenticacionDtoCarpeta;
using API_de_Reservas.DTOs.UsuarioDtoCarpeta;
using API_de_Reservas.Models;

namespace API_de_Reservas.Services.AutenticacionServiceCarpeta
{
    public interface IAutenticacionService
    {
        public Task<Result<AutenticacionRespuestaDto>> Registro(UsuarioCrearDto usuario);
        public Task<Result<AutenticacionRespuestaDto>> Login(LoginDto loginDto);
    }
}
