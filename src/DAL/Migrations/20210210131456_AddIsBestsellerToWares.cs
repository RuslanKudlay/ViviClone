using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.DAL.Migrations
{
    public partial class AddIsBestsellerToWares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBestseller",
                table: "Wares",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBestseller",
                table: "Wares");
        }
    }
}
