using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class uif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "QrCodeMaster",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bill_Id",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Collection_Id",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FPXTransactionId",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDetails",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalPaymentreceived",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionDate",
                table: "Payment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Bill_Id",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Collection_Id",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "FPXTransactionId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaymentDetails",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "TotalPaymentreceived",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "QrCodeMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
