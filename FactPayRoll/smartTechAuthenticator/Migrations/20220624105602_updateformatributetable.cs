using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateformatributetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputType",
                table: "FormProperty");

            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "FormProperty",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "FormProperty");

            migrationBuilder.AddColumn<int>(
                name: "InputType",
                table: "FormProperty",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
