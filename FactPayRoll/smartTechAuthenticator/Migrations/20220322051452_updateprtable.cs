using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateprtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMaster_QrCodeMaster_QrId",
                table: "ProductMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProductMaster_QrId",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "PurchaseFrom",
                table: "ProductMaster");

            migrationBuilder.AddColumn<int>(
                name: "QrCodeMasterId",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_QrCodeMasterId",
                table: "ProductMaster",
                column: "QrCodeMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMaster_QrCodeMaster_QrCodeMasterId",
                table: "ProductMaster",
                column: "QrCodeMasterId",
                principalTable: "QrCodeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMaster_QrCodeMaster_QrCodeMasterId",
                table: "ProductMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProductMaster_QrCodeMasterId",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "QrCodeMasterId",
                table: "ProductMaster");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseFrom",
                table: "ProductMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_QrId",
                table: "ProductMaster",
                column: "QrId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMaster_QrCodeMaster_QrId",
                table: "ProductMaster",
                column: "QrId",
                principalTable: "QrCodeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
