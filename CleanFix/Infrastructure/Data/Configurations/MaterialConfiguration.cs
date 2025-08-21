using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombre del material");

        builder.Property(m => m.Cost)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Costo del material");

        builder.Property(m => m.Available)
            .IsRequired()
            .HasComment("Disponibilidad del material");

        builder.Property(m => m.Issue)
            .IsRequired()
            .HasComment("Tipo de incidencia asociada al material");
    }
}
