using API_de_Reservas.Models;

namespace API_de_Reservas.Services.JwtServiceCarpeta
{
    public interface IJwtService
    {
        public string GenerarToken(Usuario usuario);
    }
}
