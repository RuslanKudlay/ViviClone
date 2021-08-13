using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.DAL.Migrations
{
    public partial class AddDeliveryServiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "DeclarationNumber",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryServiceId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryServices", x => x.Id);
                    table.UniqueConstraint("AK_DeliveryServices_Name", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryServiceId",
                table: "Orders",
                column: "DeliveryServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryServices_DeliveryServiceId",
                table: "Orders",
                column: "DeliveryServiceId",
                principalTable: "DeliveryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryServices_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_OrderStatusId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryServices");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryServiceId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeclarationNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryServiceId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
