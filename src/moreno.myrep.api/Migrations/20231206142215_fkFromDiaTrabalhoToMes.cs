using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moreno.myrep.api.Migrations
{
    /// <inheritdoc />
    public partial class fkFromDiaTrabalhoToMes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MesTrabalhoId",
                table: "DiaTrabalho",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DiaTrabalho_MesTrabalhoId",
                table: "DiaTrabalho",
                column: "MesTrabalhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaTrabalho_MesTrabalho_MesTrabalhoId",
                table: "DiaTrabalho",
                column: "MesTrabalhoId",
                principalTable: "MesTrabalho",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaTrabalho_MesTrabalho_MesTrabalhoId",
                table: "DiaTrabalho");

            migrationBuilder.DropIndex(
                name: "IX_DiaTrabalho_MesTrabalhoId",
                table: "DiaTrabalho");

            migrationBuilder.DropColumn(
                name: "MesTrabalhoId",
                table: "DiaTrabalho");
        }
    }
}
