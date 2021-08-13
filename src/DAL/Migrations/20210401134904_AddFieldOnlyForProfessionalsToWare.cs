using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.DAL.Migrations
{
    public partial class AddFieldOnlyForProfessionalsToWare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnlyForProfessionals",
                table: "Wares",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnlyForProfessionals",
                table: "Wares");
        }
    }
}
