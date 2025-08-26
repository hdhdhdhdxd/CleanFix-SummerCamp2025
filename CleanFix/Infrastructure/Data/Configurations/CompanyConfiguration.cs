using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Nombre de la empresa");

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(300)
            .HasComment("Dirección de la empresa");

        builder.Property(c => c.Number)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Teléfono de la empresa");

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Correo electrónico de la empresa");

        builder.Property(c => c.IssueTypeId)
            .IsRequired()
            .HasComment("Id del tipo de incidencia");

        builder.Property(c => c.Price)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Precio asociado a la empresa");

        builder.Property(c => c.WorkTime)
            .IsRequired()
            .HasComment("Tiempo de trabajo en horas");

        builder.HasOne(c => c.IssueType)
            .WithMany()
            .HasForeignKey(c => c.IssueTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
