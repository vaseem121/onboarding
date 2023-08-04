using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateorderitemtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Order_Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Order_Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "AddToCart",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "AddToCart",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "AddToCart");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "AddToCart");
        }
    }
}
