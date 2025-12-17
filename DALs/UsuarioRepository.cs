using API_de_Reservas.Models;
using Microsoft.EntityFrameworkCore;

namespace API_de_Reservas.DALs
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario?> ObtenerUsuarioPorEmail(string email)
        {
            var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u =>  u.Email == email);
            return usuarioEncontrado;
        }

        public async Task<Usuario?> ObtenerUsuarioPorId(int usuarioId)
        {
            var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
            return usuarioEncontrado;
        }
    }
}