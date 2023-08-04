using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class UpdateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authenticode",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BachNumber",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "INVNo",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseFrom",
                table: "ProductMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authenticode",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "BachNumber",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "INVNo",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "PurchaseFrom",
                table: "ProductMaster");
        }
    }
}
