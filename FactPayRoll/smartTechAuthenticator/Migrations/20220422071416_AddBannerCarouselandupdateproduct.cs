using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class AddBannerCarouselandupdateproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shipping",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tax",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalPrice",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BannerCarousel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Photo = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerCarousel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannerCarousel");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ProductMaster");
        }
    }
}
