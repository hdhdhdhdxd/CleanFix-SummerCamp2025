using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaintenanceCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintenanceCost",
                table: "Solicitations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaintenanceCost",
                table: "Solicitations",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0,
                comment: "Costo de mantenimiento asociado a la solicitud");
        }
    }
}
