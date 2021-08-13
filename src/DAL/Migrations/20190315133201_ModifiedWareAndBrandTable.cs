using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class ModifiedWareAndBrandTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Wares",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wares_BrandId",
                table: "Wares",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wares_Brands_BrandId",
                table: "Wares",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wares_Brands_BrandId",
                table: "Wares");

            migrationBuilder.DropIndex(
                name: "IX_Wares_BrandId",
                table: "Wares");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Wares");
        }
    }
}
