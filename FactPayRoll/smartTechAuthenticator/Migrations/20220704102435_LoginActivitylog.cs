using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class LoginActivitylog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginActivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    IP_Address = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: true),
                    EventReason = table.Column<int>(nullable: true),
                    EventDateTime = table.Column<DateTime>(nullable: true),
                    Location_Latitude = table.Column<string>(nullable: true),
                    Location_Longitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginActivity", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginActivity");
        }
    }
}
