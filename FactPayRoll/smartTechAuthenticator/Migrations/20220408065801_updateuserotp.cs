using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateuserotp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FPCreateTime",
                table: "CustomerInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FPExpTime",
                table: "CustomerInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ForgotPasswordOTP",
                table: "CustomerInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FPCreateTime",
                table: "CustomerInfo");

            migrationBuilder.DropColumn(
                name: "FPExpTime",
                table: "CustomerInfo");

            migrationBuilder.DropColumn(
                name: "ForgotPasswordOTP",
                table: "CustomerInfo");
        }
    }
}
