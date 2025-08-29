using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncidenceApartmentConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Incidences_ApartmentId",
                table: "Incidences",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_Apartments_ApartmentId",
                table: "Incidences",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_Apartments_ApartmentId",
                table: "Incidences");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_ApartmentId",
                table: "Incidences");
        }
    }
}
