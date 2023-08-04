using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class changeIntTOGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductMaster");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductMaster",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMaster",
                table: "ProductMaster",
                column: "Id");
        }
    }
}
