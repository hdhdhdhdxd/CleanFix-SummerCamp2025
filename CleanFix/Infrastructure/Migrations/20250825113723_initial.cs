using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloorNumber = table.Column<int>(type: "int", nullable: false, comment: "Piso del apartamento"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Dirección del apartamento"),
                    Surface = table.Column<double>(type: "float", nullable: false, comment: "Superficie del apartamento"),
                    RoomNumber = table.Column<int>(type: "int", nullable: false, comment: "Número de habitaciones"),
                    BathroomNumber = table.Column<int>(type: "int", nullable: false, comment: "Número de baños")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Nombre de la empresa"),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Dirección de la empresa"),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Teléfono de la empresa"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Correo electrónico de la empresa"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false, comment: "Id del tipo de incidencia"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Precio asociado a la empresa"),
                    WorkTime = table.Column<int>(type: "int", nullable: false, comment: "Tiempo de trabajo en horas")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false, comment: "Id del tipo de incidencia"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de la incidencia"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Estado de la incidencia"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Descripción de la incidencia"),
                    ApartmentId = table.Column<int>(type: "int", nullable: false, comment: "Id del apartamento asociado"),
                    Surface = table.Column<int>(type: "int", nullable: false, comment: "Superficie del apartamento"),
                    Priority = table.Column<int>(type: "int", nullable: false, comment: "Prioridad de la incidencia")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Tipo de incidencia")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descripción de la solicitud"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de la solicitud"),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Dirección donde se solicita el servicio"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Estado de la solicitud"),
                    MaintenanceCost = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false, comment: "Costo de mantenimiento asociado a la solicitud"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false, comment: "Id del tipo de incidencia asociada a la solicitud")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompletedTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Dirección de la tarea completada"),
                    ApartmentId = table.Column<int>(type: "int", nullable: false, comment: "Id del apartamento asociado"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de la tarea completada"),
                    Price = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false, comment: "Precio de la tarea completada"),
                    Duration = table.Column<double>(type: "float", nullable: false, comment: "Duración de la tarea en horas"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false, comment: "Id del tipo de incidencia de la tarea"),
                    IsRequest = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si la tarea fue solicitada"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Surface = table.Column<int>(type: "int", nullable: false, comment: "Superficie del apartamento")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTasks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombre del material"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Costo del material"),
                    Available = table.Column<bool>(type: "bit", nullable: false, comment: "Disponibilidad del material"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false, comment: "Id del tipo de incidencia asociada al material"),
                    CompletedTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_CompletedTasks_CompletedTaskId",
                        column: x => x.CompletedTaskId,
                        principalTable: "CompletedTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTasks_CompanyId",
                table: "CompletedTasks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTasks_UserId",
                table: "CompletedTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CompletedTaskId",
                table: "Materials",
                column: "CompletedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApartmentId",
                table: "Users",
                column: "ApartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidences");

            migrationBuilder.DropTable(
                name: "IssueTypes");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Solicitations");

            migrationBuilder.DropTable(
                name: "CompletedTasks");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Apartments");
        }
    }
}
