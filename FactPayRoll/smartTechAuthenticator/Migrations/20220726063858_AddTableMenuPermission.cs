using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class AddTableMenuPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdminId = table.Column<Guid>(nullable: false),
                    SubAdminId = table.Column<Guid>(nullable: false),
                    MallManage = table.Column<bool>(nullable: false),
                    CustomerManage = table.Column<bool>(nullable: false),
                    TicketSupportManage = table.Column<bool>(nullable: false),
                    Setting = table.Column<bool>(nullable: false),
                    AuthenticatorManage = table.Column<bool>(nullable: false),
                    FrontEndManage = table.Column<bool>(nullable: false),
                    CertificateManage = table.Column<bool>(nullable: false),
                    NewsManage = table.Column<bool>(nullable: false),
                    FormManage = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermission", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPermission");
        }
    }
}
