using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PackingSlip.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AgentCommission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    PackingSlipId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentCommission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMembership",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActivated = table.Column<bool>(nullable: false),
                    ActivatedOn = table.Column<DateTime>(nullable: true),
                    IsUpgraded = table.Column<bool>(nullable: true),
                    UpgradedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMembership", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FreeProduct",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackingSlipHeader",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackingSlipNumber = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
                    AgentName = table.Column<string>(nullable: true),
                    HasPhysicalProduct = table.Column<bool>(nullable: true),
                    HasBook = table.Column<bool>(nullable: true),
                    ActivateMembership = table.Column<bool>(nullable: true),
                    UpgradeMembership = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingSlipHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Sku = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FreeProductChild",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreeProductId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    FreeQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeProductChild", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreeProductChild_FreeProduct_FreeProductId",
                        column: x => x.FreeProductId,
                        principalSchema: "dbo",
                        principalTable: "FreeProduct",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FreeProductParent",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreeProductId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    RequiredQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeProductParent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreeProductParent_FreeProduct_FreeProductId",
                        column: x => x.FreeProductId,
                        principalSchema: "dbo",
                        principalTable: "FreeProduct",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PackingSlipItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackingSlipId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    IsFreeItem = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingSlipItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackingSlipItem_PackingSlipHeader_PackingSlipId",
                        column: x => x.PackingSlipId,
                        principalSchema: "dbo",
                        principalTable: "PackingSlipHeader",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreeProductChild_FreeProductId",
                schema: "dbo",
                table: "FreeProductChild",
                column: "FreeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FreeProductParent_FreeProductId",
                schema: "dbo",
                table: "FreeProductParent",
                column: "FreeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PackingSlipItem_PackingSlipId",
                schema: "dbo",
                table: "PackingSlipItem",
                column: "PackingSlipId");

            migrationBuilder.Sql(@"INSERT INTO dbo.Product(Name, Description, Rate) VALUES('Learning to Ski', 'Learning to Ski', 15)");
            migrationBuilder.Sql(@"INSERT INTO dbo.Product(Name, Description, Rate) VALUES('First Aid', 'First Aid', 10)");
            migrationBuilder.Sql(@"INSERT INTO dbo.FreeProduct(Description, IsActive) VALUES('Free Item For Ski', 1)");
            migrationBuilder.Sql(@"INSERT INTO dbo.FreeProductParent(FreeProductId, ProductId, RequiredQuantity) VALUES(1, 1, 1)");
            migrationBuilder.Sql(@"INSERT INTO dbo.FreeProductChild(FreeProductId, ProductId, FreeQuantity) VALUES(1, 2, 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentCommission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CustomerMembership",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FreeProductChild",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FreeProductParent",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PackingSlipItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FreeProduct",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PackingSlipHeader",
                schema: "dbo");
        }
    }
}
