using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateshipingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShippingTracking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Delivered",
                table: "ShippingTracking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "ShippingTracking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OutForDeliveryDate",
                table: "ShippingTracking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippedDate",
                table: "ShippingTracking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShippingTracking");

            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "ShippingTracking");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "ShippingTracking");

            migrationBuilder.DropColumn(
                name: "OutForDeliveryDate",
                table: "ShippingTracking");

            migrationBuilder.DropColumn(
                name: "ShippedDate",
                table: "ShippingTracking");
        }
    }
}
