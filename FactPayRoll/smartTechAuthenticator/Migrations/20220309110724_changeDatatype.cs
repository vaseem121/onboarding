using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class changeDatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DistricttName",
                table: "DistricttMaster",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DistricttName",
                table: "DistricttMaster",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
