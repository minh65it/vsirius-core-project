using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PimApi.Migrations
{
    /// <inheritdoc />
    public partial class AddVariationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VariationId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Variations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemSku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Variations");

            migrationBuilder.DropColumn(
                name: "VariationId",
                table: "Products");
        }
    }
}
