using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class addresponcetabls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckboxResponce",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormPropertyResponceId = table.Column<Guid>(nullable: false),
                    CheckboxId = table.Column<Guid>(nullable: false),
                    Check = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckboxResponce", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormPropertyResponce",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormResponceId = table.Column<Guid>(nullable: false),
                    FormPropertyId = table.Column<Guid>(nullable: false),
                    FieldType = table.Column<int>(nullable: false),
                    ResponceText = table.Column<string>(nullable: true),
                    NumberSliderValue = table.Column<int>(nullable: false),
                    DropdownValue = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormPropertyResponce", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormResponce",
                columns: table => new
                {
                    ResponseId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    FormId = table.Column<Guid>(nullable: false),
                    FormName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormResponce", x => x.ResponseId);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceResponce",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FormPropertyResponceId = table.Column<Guid>(nullable: false),
                    DropdownId = table.Column<Guid>(nullable: false),
                    ChoiceValue = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceResponce", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckboxResponce");

            migrationBuilder.DropTable(
                name: "FormPropertyResponce");

            migrationBuilder.DropTable(
                name: "FormResponce");

            migrationBuilder.DropTable(
                name: "MultipleChoiceResponce");
        }
    }
}
