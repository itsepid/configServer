using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigServer.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConfigRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Configs_UserId",
                table: "Configs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_Users_UserId",
                table: "Configs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_Users_UserId",
                table: "Configs");

            migrationBuilder.DropIndex(
                name: "IX_Configs_UserId",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Configs");
        }
    }
}
