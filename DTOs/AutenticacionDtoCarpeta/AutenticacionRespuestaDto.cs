using API_de_Reservas.DTOs.UsuarioDtoCarpeta;
using API_de_Reservas.Models;

namespace API_de_Reservas.DTOs.AutenticacionDtoCarpeta
{
    public class AutenticacionRespuestaDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }
        public int TiempoExpiracionMinutos { get; set; }

    }
}
