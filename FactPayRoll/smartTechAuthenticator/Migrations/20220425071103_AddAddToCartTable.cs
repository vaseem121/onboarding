using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class AddAddToCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddToCart",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Product = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CustId = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<string>(nullable: true),
                    Qty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddToCart", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddToCart");
        }
    }
}
