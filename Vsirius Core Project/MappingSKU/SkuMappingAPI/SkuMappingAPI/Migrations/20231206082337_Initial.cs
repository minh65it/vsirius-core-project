using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkuMappingAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SKUInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SKUQgold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUStuller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUWalmart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUAmazon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUVsirius = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SKUInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SKUInfos");
        }
    }
}
