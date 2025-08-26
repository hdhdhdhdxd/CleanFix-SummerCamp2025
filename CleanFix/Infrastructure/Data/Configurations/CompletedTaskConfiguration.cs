using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class CompletedTaskConfiguration : IEntityTypeConfiguration<CompletedTask>
{
    public void Configure(EntityTypeBuilder<CompletedTask> builder)
    {
        builder.Property(t => t.Address)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Dirección de la tarea completada");

        builder.Property(t => t.ApartmentId)
            .IsRequired()
            .HasComment("Id del apartamento asociado");

        builder.Property(t => t.CreationDate)
            .IsRequired()
            .HasComment("Fecha de la tarea completada");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Precio de la tarea completada");

        builder.Property(t => t.Duration)
            .IsRequired()
            .HasComment("Duración de la tarea en horas");

        builder.Property(t => t.IssueTypeId)
            .IsRequired()
            .HasComment("Id del tipo de incidencia de la tarea");

        builder.Property(t => t.IsRequest)
            .IsRequired()
            .HasComment("Indica si la tarea fue solicitada");

        builder.Property(t => t.Surface)
            .IsRequired()
            .HasComment("Superficie del apartamento");

        // Relaciones (Company, User, Materials) se configuran aparte si es necesario
    }
}
