using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;

namespace API_de_Reservas.Services.ReservaServiceCarpeta
{
    public interface IReservasService
    {

        public Task<Result<ReservaDto>> CrearReservaAsync(ReservaCrearDto reservaCrearDto,int usuarioId);
        public Task<Result<ReservaDto>> CancelarReserva(int reservaId, int usuarioId, string rol);
        public Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuario(int reservaUsuarioId, int usuarioId, string rol);
        public Task<Result<List<ReservaDto>>> ObtenerReservasPorRecurso(int recursoId);
    }
}
