using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class changefornew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: false),
                    QrId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMaster_ProductCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMaster_QrCodeMaster_QrId",
                        column: x => x.QrId,
                        principalTable: "QrCodeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_CategoryId",
                table: "ProductMaster",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_QrId",
                table: "ProductMaster",
                column: "QrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMaster");
        }
    }
}
