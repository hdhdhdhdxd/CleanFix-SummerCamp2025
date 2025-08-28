using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingCode",
                table: "Solicitations");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Solicitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Solicitations");

            migrationBuilder.AddColumn<string>(
                name: "BuildingCode",
                table: "Solicitations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
