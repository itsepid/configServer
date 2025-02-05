using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigServer.Web.Migrations
{
    /// <inheritdoc />
    public partial class environmentsaddedandfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigEntries_ConfigProjectId_Key",
                table: "ConfigEntries");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigEntries_ConfigProjectId_Key_Environment",
                table: "ConfigEntries",
                columns: new[] { "ConfigProjectId", "Key", "Environment" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigEntries_ConfigProjectId_Key_Environment",
                table: "ConfigEntries");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigEntries_ConfigProjectId_Key",
                table: "ConfigEntries",
                columns: new[] { "ConfigProjectId", "Key" },
                unique: true);
        }
    }
}
