using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class ModifiedWareTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Wares",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "GOWs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_GOWs_ParentId",
                table: "GOWs",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GOWs_GOWs_ParentId",
                table: "GOWs",
                column: "ParentId",
                principalTable: "GOWs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GOWs_GOWs_ParentId",
                table: "GOWs");

            migrationBuilder.DropIndex(
                name: "IX_GOWs_ParentId",
                table: "GOWs");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Wares");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "GOWs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
