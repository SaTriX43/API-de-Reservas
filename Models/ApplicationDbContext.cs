using Microsoft.EntityFrameworkCore;

namespace API_de_Reservas.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Reservas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recurso>()
                .HasMany(r => r.Reservas)
                .WithOne(r => r.Recurso)
                .HasForeignKey(r => r.RecursoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
