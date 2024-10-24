using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VsiriusDataModel.Migrations
{
    /// <inheritdoc />
    public partial class adjust_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "standard_price",
                table: "VsiriusProduct",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "VsiriusProduct",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VsiriusProduct_IsDelete",
                table: "VsiriusProduct",
                column: "IsDelete",
                filter: "IsDelete = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VsiriusProduct_IsDelete",
                table: "VsiriusProduct");

            migrationBuilder.AlterColumn<string>(
                name: "standard_price",
                table: "VsiriusProduct",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "VsiriusProduct",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
