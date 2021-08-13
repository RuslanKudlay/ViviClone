using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.DAL.Migrations
{
    public partial class AddStatusForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");
        }
    }
}
