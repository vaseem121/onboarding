using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updateprtablenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QrCodeMasterId",
                table: "ProductMaster",
                type: "int",
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
    }
}
