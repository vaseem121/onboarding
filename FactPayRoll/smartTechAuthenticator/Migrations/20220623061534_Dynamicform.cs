using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class Dynamicform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckBox",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormPropertyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RowNumber = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dropdown",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormPropertyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RowNumber = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dropdown", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FormProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    FormId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    InputType = table.Column<int>(nullable: false),
                    RowNumber = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    Captcha = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormPropertyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RowNumber = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoice", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckBox");

            migrationBuilder.DropTable(
                name: "Dropdown");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "FormProperty");

            migrationBuilder.DropTable(
                name: "MultipleChoice");
        }
    }
}
