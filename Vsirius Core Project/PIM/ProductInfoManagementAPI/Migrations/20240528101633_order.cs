using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PimApi.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemSku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Marketplace = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShipToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipToPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipToAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipToZip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => new { x.OrderId, x.ItemSku, x.Marketplace });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
