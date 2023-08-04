using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class updatetable08072022_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificationAd",
                table: "TicketMessageSystem",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationCust",
                table: "TicketMessageSystem",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationAd",
                table: "TicketMessageSystem");

            migrationBuilder.DropColumn(
                name: "NotificationCust",
                table: "TicketMessageSystem");
        }
    }
}
