using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API_de_Reservas.DALs.ReservaRepositoryCarpeta
{
    public class ReservasRepository : IReservasRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservasRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public Reserva CrearReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            return reserva;
        }

        public async Task<Reserva?> ObtenerReservaPorIdAsync(int reservaId)
        {
            var reservaEncontrada = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == reservaId);
            return reservaEncontrada;
        }

        public async Task<List<Reserva>> ObtenerReservasPorUsuarioAsync(int usuarioId) {
            var reservarPorUsuario = await _context.Reservas.Where(r => r.UsuarioId == usuarioId).ToListAsync();
            return reservarPorUsuario;
        }

        public async Task<List<Reserva>> ObtenerReservasPorRecursoAsync(int recursoId)
        {
            var reservasPorRecurso = await _context.Reservas.Where(r => r.RecursoId == recursoId).ToListAsync();
            return reservasPorRecurso;
        }

        public async Task<List<Reserva>> ObtenerTodasLasReservasAsync(int page, int pageSize, DateTime? fechaInicio, DateTime? fechaFinal)
        {
            var query = _context.Reservas.AsQueryable();

            if(fechaInicio.HasValue)
            {
               query =  query.Where(r => r.FechaCreacion >= fechaInicio);
            }

            if (fechaFinal.HasValue)
            {
               query =  query.Where(r => r.FechaCreacion <= fechaFinal);
            }

            query = query
                .OrderByDescending(r => r.FechaCreacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
