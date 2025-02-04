using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigServer.Web.Migrations
{
    /// <inheritdoc />
    public partial class databaseUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    PasskeyHash = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigEntries_ConfigProjects_ConfigProjectId",
                        column: x => x.ConfigProjectId,
                        principalTable: "ConfigProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigEntries_ConfigProjectId_Key",
                table: "ConfigEntries",
                columns: new[] { "ConfigProjectId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigProjects_ProjectName",
                table: "ConfigProjects",
                column: "ProjectName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigEntries");

            migrationBuilder.DropTable(
                name: "ConfigProjects");
        }
    }
}
