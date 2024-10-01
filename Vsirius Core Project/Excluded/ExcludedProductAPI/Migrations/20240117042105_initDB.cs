using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcludedProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcludedProducts",
                columns: table => new
                {
                    MarketplaceSKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Marketplace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierSKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExclusionType = table.Column<int>(type: "int", nullable: false),
                    ExclusionNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludedProducts", x => x.MarketplaceSKU);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcludedProducts");
        }
    }
}
