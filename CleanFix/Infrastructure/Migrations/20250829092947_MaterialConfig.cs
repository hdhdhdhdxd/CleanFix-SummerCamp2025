using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MaterialConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_Apartments_ApartmentId",
                table: "Incidences");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_ApartmentId",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "Incidences");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPerSquareMeter",
                table: "Materials",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Surface",
                table: "Incidences",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Superficie del apartamento");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Incidences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "Surface",
                table: "CompletedTasks",
                type: "float",
                nullable: false,
                comment: "Superficie del apartamento",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Superficie del apartamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerSquareMeter",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Incidences");

            migrationBuilder.AlterColumn<int>(
                name: "Surface",
                table: "Incidences",
                type: "int",
                nullable: false,
                comment: "Superficie del apartamento",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "Incidences",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Id del apartamento asociado");

            migrationBuilder.AlterColumn<int>(
                name: "Surface",
                table: "CompletedTasks",
                type: "int",
                nullable: false,
                comment: "Superficie del apartamento",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Superficie del apartamento");

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
    }
}
