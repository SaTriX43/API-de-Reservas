using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public interface IReservasRepository
    {
        public Task<List<Reserva>> ObtenerReservasPorTipo(string tipoReserva);
        public Task<Reserva> CrearReserva(Reserva reserva);
    }
}
