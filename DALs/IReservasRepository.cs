using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public interface IReservasRepository
    {
        public Task<Reserva> CrearReserva(Reserva reserva);
        public Task<Reserva?> ObtenerReservaPorId(int reservaId);
        public Task<Reserva> CancelarReserva(int reservaId);
        public Task<List<Reserva>> ObtenerReservasPorUsuario(int usuarioId);
        public Task<List<Reserva>> ObtenerReservasPorRecurso(int recursoId);
    }
}
