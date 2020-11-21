using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aqua.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Aqua");

            migrationBuilder.CreateTable(
                name: "Animal",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "string", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.Id);
                    table.CheckConstraint("CK_Inventory_Quantity_Nonnegative", "[Quantity] >= 0");
                    table.ForeignKey(
                        name: "FK_InventoryItem_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "Aqua",
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItem_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "Aqua",
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDateTime())"),
                    Total = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.CheckConstraint("CK_Order_Total_Nonnegative", "[Total] >= 0");
                    table.ForeignKey(
                        name: "FK_Order_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Aqua",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "Aqua",
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "Aqua",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.CheckConstraint("CK_OrderItem_Quantity_Nonnegative", "[Quantity] >= 0");
                    table.CheckConstraint("CK_OrderItem_Total_Nonnegative", "[Total] >= 0");
                    table.ForeignKey(
                        name: "FK_OrderItem_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "Aqua",
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Aqua",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_AnimalId",
                schema: "Aqua",
                table: "InventoryItem",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_LocationId",
                schema: "Aqua",
                table: "InventoryItem",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                schema: "Aqua",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_LocationId",
                schema: "Aqua",
                table: "Order",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_AnimalId",
                schema: "Aqua",
                table: "OrderItem",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "Aqua",
                table: "OrderItem",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItem",
                schema: "Aqua");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "Aqua");

            migrationBuilder.DropTable(
                name: "Animal",
                schema: "Aqua");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Aqua");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Aqua");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "Aqua");
        }
    }
}
