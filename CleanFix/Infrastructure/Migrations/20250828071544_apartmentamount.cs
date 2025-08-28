using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class apartmentamount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentAmount",
                table: "Solicitations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTasks_ApartmentId",
                table: "CompletedTasks",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks");

            migrationBuilder.DropIndex(
                name: "IX_CompletedTasks_ApartmentId",
                table: "CompletedTasks");

            migrationBuilder.DropColumn(
                name: "ApartmentAmount",
                table: "Solicitations");
        }
    }
}
