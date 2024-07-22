using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserInteraction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: false),
                    played_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    cards_played = table.Column<string>(type: "jsonb", nullable: false),
                    result = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_histories_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    avatar_url = table.Column<string>(type: "VARCHAR", maxLength: 64, nullable: false, defaultValueSql: "'img/avatars/default_avatar.png'::text"),
                    player_name = table.Column<string>(type: "VARCHAR", maxLength: 25, nullable: false),
                    matches = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    loses = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    wins = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    draws = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    all_exp = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    current_exp = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    target_exp = table.Column<int>(type: "integer", nullable: false, defaultValue: 100),
                    money = table.Column<int>(type: "integer", nullable: false, defaultValue: 1000),
                    likes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    has21 = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    has_gold21 = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_infos_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    FriendsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerInfoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.FriendsId, x.PlayerInfoId });
                    table.ForeignKey(
                        name: "FK_Friends_player_infos_FriendsId",
                        column: x => x.FriendsId,
                        principalTable: "player_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_player_infos_PlayerInfoId",
                        column: x => x.PlayerInfoId,
                        principalTable: "player_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_PlayerInfoId",
                table: "Friends",
                column: "PlayerInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "player_histories");

            migrationBuilder.DropTable(
                name: "player_infos");
        }
    }
}
