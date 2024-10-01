using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VsiriusSystem.Migrations
{
    /// <inheritdoc />
    public partial class add_jobinfo_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobInfos",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tittle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cron = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRestartDocker = table.Column<bool>(type: "bit", nullable: false),
                    IsStop = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobInfos", x => x.JobId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobInfos");
        }
    }
}
