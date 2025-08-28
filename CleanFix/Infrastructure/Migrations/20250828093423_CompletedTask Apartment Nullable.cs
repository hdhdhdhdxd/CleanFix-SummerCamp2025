using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompletedTaskApartmentNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks");

            migrationBuilder.AlterColumn<int>(
                name: "ApartmentId",
                table: "CompletedTasks",
                type: "int",
                nullable: true,
                comment: "Id del apartamento asociado",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id del apartamento asociado");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks");

            migrationBuilder.AlterColumn<int>(
                name: "ApartmentId",
                table: "CompletedTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Id del apartamento asociado",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Id del apartamento asociado");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedTasks_Apartments_ApartmentId",
                table: "CompletedTasks",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
