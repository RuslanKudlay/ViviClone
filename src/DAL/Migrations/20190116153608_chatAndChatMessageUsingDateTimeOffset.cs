using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class chatAndChatMessageUsingDateTimeOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TimeToCloseChat",
                table: "Chats",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Chats",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "ChatMessages",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeToCloseChat",
                table: "Chats",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Chats",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ChatMessages",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
