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

        public async Task<List<Reserva>> ObtenerReservasPorTipo(string tipoRecurso)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Recurso)
                .Where(r => r.Recurso.Tipo.ToString() == tipoRecurso).ToListAsync();

            return reservas;
                
        }
        public async Task<Reserva> CrearReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }
    }
}
