using CampusLoveSimonYSantiago.Modules;
using Microsoft.EntityFrameworkCore;

namespace CampusLoveSimonYSantiago.Shared
{
    public class AppDbContext : DbContext
    {
        public DbSet<PersonaObject> Personas { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Interes> Intereses { get; set; }
        public DbSet<PersonaInteres> PersonaIntereses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonaObject>()
                .HasOne(p => p.Carrera)
                .WithMany(c => c.Personas)
                .HasForeignKey(p => p.CarreraId);

            modelBuilder.Entity<PersonaInteres>()
                .HasKey(pi => new { pi.PersonaId, pi.InteresId });

            modelBuilder.Entity<PersonaInteres>()
                .HasOne(pi => pi.Persona)
                .WithMany(p => p.PersonaIntereses)
                .HasForeignKey(pi => pi.PersonaId);

            modelBuilder.Entity<PersonaInteres>()
                .HasOne(pi => pi.Interes)
                .WithMany(i => i.PersonaIntereses)
                .HasForeignKey(pi => pi.InteresId);

            base.OnModelCreating(modelBuilder);
        }
    }
}