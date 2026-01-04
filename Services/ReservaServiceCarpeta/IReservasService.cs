using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;

namespace API_de_Reservas.Services.ReservaServiceCarpeta
{
    public interface IReservasService
    {
        //Usuario
        public Task<Result<ReservaDto>> CrearReservaAsync(ReservaCrearDto reservaCrearDto,int usuarioId);
        public Task<Result> CancelarReservaUsuarioAsync(int reservaId, int usuarioId);
        public Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuarioIdAsync(int usuarioId);
        public Task<Result<List<ReservaDto>>> ObtenerReservasPorRecurso(int recursoId);


        //Admin
        public Task<Result> CancelarReservaAdminAsync(int reservaId);
        public Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuarioIdAdminAsync(int usuarioId);
    }
}
