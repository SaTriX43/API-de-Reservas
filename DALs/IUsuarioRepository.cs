using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> ObtenerUsuarioPorEmail(string email);
        public Task<Usuario?> ObtenerUsuarioPorId(int usuarioId);
    }
}
