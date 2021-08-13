using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class BrandsGowsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Brand",
                table: "GOWs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BrandsGOWs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(nullable: false),
                    GowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandsGOWs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandsGOWs_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandsGOWs_GOWs_GowId",
                        column: x => x.GowId,
                        principalTable: "GOWs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandsGOWs_BrandId",
                table: "BrandsGOWs",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandsGOWs_GowId",
                table: "BrandsGOWs",
                column: "GowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandsGOWs");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "GOWs");
        }
    }
}
