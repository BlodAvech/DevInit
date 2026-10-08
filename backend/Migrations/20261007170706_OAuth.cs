using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class OAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userOAuth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    provider_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userOAuth", x => x.id);
                    table.ForeignKey(
                        name: "FK_userOAuth_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userOAuth_provider_id",
                table: "userOAuth",
                column: "provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userOAuth_user_id",
                table: "userOAuth",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userOAuth");
        }
    }
}
