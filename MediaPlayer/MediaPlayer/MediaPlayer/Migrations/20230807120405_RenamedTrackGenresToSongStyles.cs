using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaPlayer.Migrations
{
    /// <inheritdoc />
    public partial class RenamedTrackGenresToSongStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackGenres_Genres_GenreID",
                table: "TrackGenres");

            migrationBuilder.DropIndex(
                name: "IX_TrackGenres_GenreID",
                table: "TrackGenres");

            migrationBuilder.AddColumn<int>(
                name: "TrackGenreGenreID",
                table: "TrackGenres",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrackGenres_TrackGenreGenreID",
                table: "TrackGenres",
                column: "TrackGenreGenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackGenres_Genres_TrackGenreGenreID",
                table: "TrackGenres",
                column: "TrackGenreGenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackGenres_Genres_TrackGenreGenreID",
                table: "TrackGenres");

            migrationBuilder.DropIndex(
                name: "IX_TrackGenres_TrackGenreGenreID",
                table: "TrackGenres");

            migrationBuilder.DropColumn(
                name: "TrackGenreGenreID",
                table: "TrackGenres");

            migrationBuilder.CreateIndex(
                name: "IX_TrackGenres_GenreID",
                table: "TrackGenres",
                column: "GenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackGenres_Genres_GenreID",
                table: "TrackGenres",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
