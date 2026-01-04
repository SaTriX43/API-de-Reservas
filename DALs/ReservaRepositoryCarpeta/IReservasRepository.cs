using API_de_Reservas.Models;

namespace API_de_Reservas.DALs.ReservaRepositoryCarpeta
{
    public interface IReservasRepository
    {
        public Reserva CrearReserva(Reserva reserva);
        public Task<Reserva?> ObtenerReservaPorIdAsync(int reservaId);
        public Task<List<Reserva>> ObtenerReservasPorUsuarioAsync(int usuarioId);
        public Task<List<Reserva>> ObtenerReservasPorRecursoAsync(int recursoId);
    }
}
