using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class websiteadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "CustomerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CustomerInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "CustomerInfo");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "CustomerInfo");
        }
    }
}
