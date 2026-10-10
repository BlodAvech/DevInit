using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userOAuth_provider_id",
                table: "userOAuth");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userOAuth_provider_provider_id",
                table: "userOAuth",
                columns: new[] { "provider", "provider_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_userOAuth_provider_provider_id",
                table: "userOAuth");

            migrationBuilder.CreateIndex(
                name: "IX_userOAuth_provider_id",
                table: "userOAuth",
                column: "provider_id",
                unique: true);
        }
    }
}
