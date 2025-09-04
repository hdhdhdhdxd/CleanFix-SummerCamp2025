using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Solicitations");

            migrationBuilder.AddColumn<string>(
                name: "BuildingCode",
                table: "Solicitations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Código del edificio de speculab");

            migrationBuilder.AddColumn<int>(
                name: "IncidenceId",
                table: "Incidences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingCode",
                table: "Solicitations");

            migrationBuilder.DropColumn(
                name: "IncidenceId",
                table: "Incidences");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Solicitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
