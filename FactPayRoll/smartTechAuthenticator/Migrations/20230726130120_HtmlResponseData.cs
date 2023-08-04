using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class HtmlResponseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HtmlResponseData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    HtmlResponse = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlResponseData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HtmlResponseData");
        }
    }
}
