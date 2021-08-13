using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class UpdateWaresTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WareImage",
                table: "Wares",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>("WareImageTmp", "Wares", "varbinary(max)", nullable: true);
            migrationBuilder.Sql("Update Wares SET WareImageTmp = Convert(varbinary, WareImage)");
            migrationBuilder.DropColumn("WareImage", "Wares");
            migrationBuilder.RenameColumn("WareImageTmp", "Wares", "WareImage");
        }
    }
}
