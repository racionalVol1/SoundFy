using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundFy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoPlaylistsEAssociacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistMusicas",
                table: "PlaylistMusicas");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistMusicaId",
                table: "PlaylistMusicas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistMusicas",
                table: "PlaylistMusicas",
                column: "PlaylistMusicaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMusicas_PlaylistId",
                table: "PlaylistMusicas",
                column: "PlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistMusicas",
                table: "PlaylistMusicas");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistMusicas_PlaylistId",
                table: "PlaylistMusicas");

            migrationBuilder.DropColumn(
                name: "PlaylistMusicaId",
                table: "PlaylistMusicas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistMusicas",
                table: "PlaylistMusicas",
                columns: new[] { "PlaylistId", "MusicaId" });
        }
    }
}
