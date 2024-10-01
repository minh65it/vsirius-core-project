using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAiApi.Migrations
{
    /// <inheritdoc />
    public partial class createProductAITable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAIs",
                columns: table => new
                {
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint1 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint2 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint3 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint5 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint6 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint7 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint8 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BulletPoint9 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsReview = table.Column<bool>(type: "bit", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAIs", x => x.SKU);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAIs");
        }
    }
}
