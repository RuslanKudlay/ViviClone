using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Application.DAL.Migrations
{
    public partial class AddGOWAndCategoryValuesWCVTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupOfWaresId",
                table: "Wares",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryValueses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryValueses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryValueses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GOWs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOWs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WCV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryValueId = table.Column<int>(type: "int", nullable: false),
                    WareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WCV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WCV_CategoryValueses_CategoryValueId",
                        column: x => x.CategoryValueId,
                        principalTable: "CategoryValueses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WCV_Wares_WareId",
                        column: x => x.WareId,
                        principalTable: "Wares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wares_GroupOfWaresId",
                table: "Wares",
                column: "GroupOfWaresId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValueses_CategoryId",
                table: "CategoryValueses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WCV_CategoryValueId",
                table: "WCV",
                column: "CategoryValueId");

            migrationBuilder.CreateIndex(
                name: "IX_WCV_WareId",
                table: "WCV",
                column: "WareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wares_GOWs_GroupOfWaresId",
                table: "Wares",
                column: "GroupOfWaresId",
                principalTable: "GOWs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wares_GOWs_GroupOfWaresId",
                table: "Wares");

            migrationBuilder.DropTable(
                name: "GOWs");

            migrationBuilder.DropTable(
                name: "WCV");

            migrationBuilder.DropTable(
                name: "CategoryValueses");

            migrationBuilder.DropIndex(
                name: "IX_Wares_GroupOfWaresId",
                table: "Wares");

            migrationBuilder.DropColumn(
                name: "GroupOfWaresId",
                table: "Wares");
        }
    }
}
