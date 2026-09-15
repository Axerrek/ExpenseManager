using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpenseManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Category", "Date", "Description", "Price" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zakupy spożywcze w Biedronce", 145.80m },
                    { 2, 1, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rachunek za prąd", 230.00m },
                    { 3, 2, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilet do kina", 35.50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");
        }
    }
}
