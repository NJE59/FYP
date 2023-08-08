using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaPlayer.Migrations
{
    /// <inheritdoc />
    public partial class FixedSongStylesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackGenres_Genres_TrackGenreGenreID",
                table: "TrackGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackGenres_Tracks_TrackID",
                table: "TrackGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackGenres",
                table: "TrackGenres");

            migrationBuilder.RenameTable(
                name: "TrackGenres",
                newName: "SongStyles");

            migrationBuilder.RenameIndex(
                name: "IX_TrackGenres_TrackID",
                table: "SongStyles",
                newName: "IX_SongStyles_TrackID");

            migrationBuilder.RenameIndex(
                name: "IX_TrackGenres_TrackGenreGenreID",
                table: "SongStyles",
                newName: "IX_SongStyles_TrackGenreGenreID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongStyles",
                table: "SongStyles",
                column: "TrackGenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_SongStyles_Genres_TrackGenreGenreID",
                table: "SongStyles",
                column: "TrackGenreGenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongStyles_Tracks_TrackID",
                table: "SongStyles",
                column: "TrackID",
                principalTable: "Tracks",
                principalColumn: "TrackID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongStyles_Genres_TrackGenreGenreID",
                table: "SongStyles");

            migrationBuilder.DropForeignKey(
                name: "FK_SongStyles_Tracks_TrackID",
                table: "SongStyles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongStyles",
                table: "SongStyles");

            migrationBuilder.RenameTable(
                name: "SongStyles",
                newName: "TrackGenres");

            migrationBuilder.RenameIndex(
                name: "IX_SongStyles_TrackID",
                table: "TrackGenres",
                newName: "IX_TrackGenres_TrackID");

            migrationBuilder.RenameIndex(
                name: "IX_SongStyles_TrackGenreGenreID",
                table: "TrackGenres",
                newName: "IX_TrackGenres_TrackGenreGenreID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackGenres",
                table: "TrackGenres",
                column: "TrackGenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackGenres_Genres_TrackGenreGenreID",
                table: "TrackGenres",
                column: "TrackGenreGenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackGenres_Tracks_TrackID",
                table: "TrackGenres",
                column: "TrackID",
                principalTable: "Tracks",
                principalColumn: "TrackID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
