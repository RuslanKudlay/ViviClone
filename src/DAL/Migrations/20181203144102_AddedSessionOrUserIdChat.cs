using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class AddedSessionOrUserIdChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatSessionId",
                table: "Chats",
                newName: "SessionOrUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionOrUserId",
                table: "Chats",
                newName: "ChatSessionId");
        }
    }
}
