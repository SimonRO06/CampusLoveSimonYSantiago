using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApuntesCS.Modules.Entidad.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApuntesCS.Shared.Configurations
{
    public class EntidadConfiguration
    {
        public void Configure(EntityTypeBuilder<Entidad> builder)
        {
            builder.ToTable("entidad"); // Configura el nombre de la tabla en la base de datos

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nombre) // Configura la propiedad Nombre
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}