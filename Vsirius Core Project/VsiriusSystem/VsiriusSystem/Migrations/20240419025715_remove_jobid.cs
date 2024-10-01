using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VsiriusSystem.Migrations
{
    /// <inheritdoc />
    public partial class remove_jobid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobInfos",
                table: "JobInfos");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "JobInfos");

            migrationBuilder.AlterColumn<string>(
                name: "Tittle",
                table: "JobInfos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobInfos",
                table: "JobInfos",
                column: "Tittle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobInfos",
                table: "JobInfos");

            migrationBuilder.AlterColumn<string>(
                name: "Tittle",
                table: "JobInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "JobInfos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobInfos",
                table: "JobInfos",
                column: "JobId");
        }
    }
}
