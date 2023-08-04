using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartTechAuthenticator.Migrations
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 400, nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 6, nullable: false),
                    Address1 = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Address2 = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Address3 = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    CreatedTS = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    LoginID = table.Column<string>(maxLength: 50, nullable: false),
                    LoginPass = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddressMaster",
                columns: table => new
                {
                    CustomerAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(nullable: false),
                    DistricttId = table.Column<int>(nullable: false),
                    PostCode = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddressMaster", x => x.CustomerAddressId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    NRIC = table.Column<string>(unicode: false, maxLength: 12, nullable: false),
                    MobileNo = table.Column<string>(unicode: false, maxLength: 13, nullable: false),
                    DeviceId = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Address1 = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Address3 = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CompanyId = table.Column<int>(nullable: false, defaultValueSql: "('0')"),
                    Email = table.Column<string>(unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('')"),
                    UserPass = table.Column<string>(maxLength: 20, nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateMater",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMater", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestkitCheckList",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<Guid>(nullable: false),
                    CreatedTS = table.Column<DateTime>(type: "datetime", nullable: false),
                    QRCode = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestkitCheckList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TestKits",
                columns: table => new
                {
                    QRCode = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Product = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CustId = table.Column<Guid>(nullable: true),
                    CreatedTS = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntigenKits", x => x.QRCode);
                });

            migrationBuilder.CreateTable(
                name: "TrackingForms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(unicode: false, maxLength: 12, nullable: false),
                    LotNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Time = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Date = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Place = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    TestkitId = table.Column<Guid>(nullable: false),
                    TestResults = table.Column<string>(maxLength: 10, nullable: false),
                    AntigenType = table.Column<string>(maxLength: 50, nullable: false),
                    CustId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DistricttMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistricttName = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistricttMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistricttMaster_StateMater_StateId",
                        column: x => x.StateId,
                        principalTable: "StateMater",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistricttMaster_StateId",
                table: "DistricttMaster",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyInfo");

            migrationBuilder.DropTable(
                name: "CustomerAddressMaster");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "DistricttMaster");

            migrationBuilder.DropTable(
                name: "TestkitCheckList");

            migrationBuilder.DropTable(
                name: "TestKits");

            migrationBuilder.DropTable(
                name: "TrackingForms");

            migrationBuilder.DropTable(
                name: "StateMater");
        }
    }
}
