using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class IncidenceConfiguration : IEntityTypeConfiguration<Incidence>
{
    public void Configure(EntityTypeBuilder<Incidence> builder)
    {
        builder.Property(i => i.Type)
            .IsRequired()
            .HasComment("Tipo de incidencia");

        builder.Property(i => i.Date)
            .IsRequired()
            .HasComment("Fecha de la incidencia");

        builder.Property(i => i.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Estado de la incidencia");

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Descripción de la incidencia");

        builder.Property(i => i.ApartmentId)
            .IsRequired()
            .HasComment("Id del apartamento asociado");

        builder.Property(i => i.Surface)
            .IsRequired()
            .HasComment("Superficie del apartamento");

        builder.Property(i => i.Priority)
            .IsRequired()
            .HasComment("Prioridad de la incidencia");
    }
}
