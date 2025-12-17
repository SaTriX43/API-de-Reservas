using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models;

namespace API_de_Reservas.Services
{
    public interface IReservasService
    {

        public Task<Result<ReservaDto>> CrearReserva(ReservaCrearDto reservaCrearDto);
    }
}
