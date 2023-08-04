using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class colorandsizeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductSize");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductSize",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "ProductMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "ProductMaster");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
