using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class SolicitationConfiguration : IEntityTypeConfiguration<Solicitation>
{
    public void Configure(EntityTypeBuilder<Solicitation> builder)
    {
        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .HasComment("Descripción de la solicitud");

        builder.Property(s => s.Date)
            .IsRequired()
            .HasComment("Fecha de la solicitud");

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(300)
            .HasComment("Dirección donde se solicita el servicio");

        builder.Property(s => s.MaintenanceCost)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Costo de mantenimiento asociado a la solicitud");

        builder.Property(s => s.IssueTypeId)
            .IsRequired()
            .HasComment("Id del tipo de incidencia");

        builder.Property(s => s.BuildingCode)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Código del edificio de speculab");

        builder.HasOne(s => s.IssueType)
            .WithMany()
            .HasForeignKey(s => s.IssueTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
