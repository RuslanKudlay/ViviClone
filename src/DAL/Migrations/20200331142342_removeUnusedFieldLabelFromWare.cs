using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.DAL.Migrations
{
    public partial class removeUnusedFieldLabelFromWare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Wares");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Wares",
                nullable: true);
        }
    }
}
