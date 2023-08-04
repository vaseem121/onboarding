using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateFname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FName",
                table: "CustomerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LName",
                table: "CustomerInfo",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CustomerInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FName",
                table: "CustomerInfo");

            migrationBuilder.DropColumn(
                name: "LName",
                table: "CustomerInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CustomerInfo");
        }
    }
}
