using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EbayCoreApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemID",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionID",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "OrderDetails");
        }
    }
}
