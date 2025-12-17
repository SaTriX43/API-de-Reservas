using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API_de_Reservas.DALs
{
    public class ReservasRepository : IReservasRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservasRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<Reserva> CrearReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<Reserva?> ObtenerReservaPorId(int reservaId)
        {
            var reservaEncontrada = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == reservaId);
            return reservaEncontrada;
        }

        public async Task<Reserva> CancelarReserva(int reservaId)
        {
            var reservaEncontrada = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == reservaId);

            reservaEncontrada.Estado = EstadoReserva.Cancelada;

            await _context.SaveChangesAsync();

            return reservaEncontrada;
        }

        public async Task<List<Reserva>> ObtenerReservasPorUsuario(int usuarioId) {
            var reservarPorUsuario = await _context.Reservas.Where(r => r.UsuarioId == usuarioId).ToListAsync();
            return reservarPorUsuario;
        }
    }
}
