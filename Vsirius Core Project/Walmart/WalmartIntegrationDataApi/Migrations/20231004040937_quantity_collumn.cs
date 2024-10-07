using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalmartIntegrationDataApi.Migrations
{
    /// <inheritdoc />
    public partial class quantity_collumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "quantity",
                table: "Walmarts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Walmarts");
        }
    }
}
