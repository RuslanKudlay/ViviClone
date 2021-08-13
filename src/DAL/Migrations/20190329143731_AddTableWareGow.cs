using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class AddTableWareGow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wares_GOWs_GroupOfWaresId",
                table: "Wares");

            migrationBuilder.DropIndex(
                name: "IX_Wares_GroupOfWaresId",
                table: "Wares");

            migrationBuilder.DropColumn(
                name: "GroupOfWaresId",
                table: "Wares");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "GOWs");

            migrationBuilder.CreateTable(
                name: "WareGOWs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GOWId = table.Column<int>(nullable: false),
                    WareId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareGOWs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareGOWs_GOWs_GOWId",
                        column: x => x.GOWId,
                        principalTable: "GOWs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareGOWs_Wares_WareId",
                        column: x => x.WareId,
                        principalTable: "Wares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareGOWs_GOWId",
                table: "WareGOWs",
                column: "GOWId");

            migrationBuilder.CreateIndex(
                name: "IX_WareGOWs_WareId",
                table: "WareGOWs",
                column: "WareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WareGOWs");

            migrationBuilder.AddColumn<int>(
                name: "GroupOfWaresId",
                table: "Wares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Brand",
                table: "GOWs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wares_GroupOfWaresId",
                table: "Wares",
                column: "GroupOfWaresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wares_GOWs_GroupOfWaresId",
                table: "Wares",
                column: "GroupOfWaresId",
                principalTable: "GOWs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
