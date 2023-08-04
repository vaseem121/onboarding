using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateproductTable07072022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormIds",
                table: "CustomerInfo");

            migrationBuilder.AddColumn<string>(
                name: "FormIds",
                table: "ProductMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormIds",
                table: "ProductMaster");

            migrationBuilder.AddColumn<string>(
                name: "FormIds",
                table: "CustomerInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
