using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QgoldIntegrationDataApi.Migrations
{
    /// <inheritdoc />
    public partial class update_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QgoldCsvDatas",
                table: "QgoldCsvDatas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QgoldCsvDatas");

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "QgoldCsvDatas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "QgoldCsvDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "QgoldCsvDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QgoldCsvDatas",
                table: "QgoldCsvDatas",
                column: "Item");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QgoldCsvDatas",
                table: "QgoldCsvDatas");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "QgoldCsvDatas");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "QgoldCsvDatas");

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "QgoldCsvDatas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "QgoldCsvDatas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_QgoldCsvDatas",
                table: "QgoldCsvDatas",
                column: "Id");
        }
    }
}
