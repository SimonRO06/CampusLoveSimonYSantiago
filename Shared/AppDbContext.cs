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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Match> Matches { get; set; }

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
            
            modelBuilder.Entity<Match>()
            .HasOne(m => m.Persona1)
            .WithMany()
            .HasForeignKey(m => m.Persona1Id)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Persona2)
                .WithMany()
                .HasForeignKey(m => m.Persona2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasIndex(m => new { m.Persona1Id, m.Persona2Id })
                .IsUnique();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.PersonaQueDaLike)
                .WithMany()
                .HasForeignKey(l => l.PersonaQueDaLikeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.PersonaQueRecibeLike)
                .WithMany()
                .HasForeignKey(l => l.PersonaQueRecibeLikeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.PersonaQueDaLikeId, l.PersonaQueRecibeLikeId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}