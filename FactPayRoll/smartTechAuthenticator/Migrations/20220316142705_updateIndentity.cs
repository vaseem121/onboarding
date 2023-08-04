using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateIndentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductMaster");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductMaster",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductMaster");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductMaster",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster",
                column: "ProductId");
        }
    }
}
