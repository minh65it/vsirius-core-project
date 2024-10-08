using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtsyIntegrationDataAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refund_Order_receipt_id",
                table: "Refund");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Order_receipt_id",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Order_receipt_id",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "receipt_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refund_Order_receipt_id",
                table: "Refund",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "receipt_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Order_receipt_id",
                table: "Shipment",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "receipt_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Order_receipt_id",
                table: "Transaction",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "receipt_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refund_Order_receipt_id",
                table: "Refund");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Order_receipt_id",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Order_receipt_id",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refund_Order_receipt_id",
                table: "Refund",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Order_receipt_id",
                table: "Shipment",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Order_receipt_id",
                table: "Transaction",
                column: "receipt_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
