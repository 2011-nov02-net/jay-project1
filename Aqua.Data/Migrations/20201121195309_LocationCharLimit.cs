using Microsoft.EntityFrameworkCore.Migrations;

namespace Aqua.Data.Migrations
{
    public partial class LocationCharLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Aqua",
                table: "Animal",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Aqua",
                table: "Animal",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Aqua",
                table: "Animal",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Aqua",
                table: "Location",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Aqua",
                table: "Location",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Aqua",
                table: "Location",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Aqua",
                table: "Location",
                type: "nvarchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.InsertData(
                schema: "Aqua",
                table: "Animal",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Fish", 3m },
                    { 2, "Shark", 50m },
                    { 3, "Dolphin", 1000m }
                });

            migrationBuilder.InsertData(
                schema: "Aqua",
                table: "Location",
                columns: new[] { "Id", "City" },
                values: new object[,]
                {
                    { 1, "Nyc" },
                    { 2, "Seoul" }
                });
        }
    }
}
