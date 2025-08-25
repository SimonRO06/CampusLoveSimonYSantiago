using CampusLoveExamen.Modules.Persona.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApuntesCS.Shared.Configurations
{
    public class EntidadConfiguration
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona"); // Configura el nombre de la tabla en la base de datos

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Edad)
                .IsRequired();

            builder.Property(p => p.Genero)
                .IsRequired();

            builder.Property(p => p.Carrera)
                .IsRequired();

            builder.Property(p => p.Enfoque)
                .HasMaxLength(100);

            builder.Property(p => p.InteresFavorito)
                .IsRequired();

            builder.Property(p => p.Intereses)
                .HasColumnType("TEXT")
                .IsRequired();;

            builder.Property(p => p.Frase)
                .HasColumnType("TEXT")
                .IsRequired();;

            builder.Property(p => p.Likes);
        }
    }
}