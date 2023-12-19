using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moreno.myrep.api.Migrations
{
    /// <inheritdoc />
    public partial class MigMyRep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaTrabalho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataDiaTrabalho = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaTrabalho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MesTrabalho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrestadorServico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consultoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InicioPeriodo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminoPeriodo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MesTrabalho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apontamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etapa = table.Column<byte>(type: "tinyint", nullable: false),
                    DiaTrabalhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apontamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apontamento_DiaTrabalho_DiaTrabalhoId",
                        column: x => x.DiaTrabalhoId,
                        principalTable: "DiaTrabalho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apontamento_DiaTrabalhoId",
                table: "Apontamento",
                column: "DiaTrabalhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apontamento");

            migrationBuilder.DropTable(
                name: "MesTrabalho");

            migrationBuilder.DropTable(
                name: "DiaTrabalho");
        }
    }
}
