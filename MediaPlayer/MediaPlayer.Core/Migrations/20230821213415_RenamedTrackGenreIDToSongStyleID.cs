using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaPlayer.Core.Migrations
{
    /// <inheritdoc />
    public partial class RenamedTrackGenreIDToSongStyleID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackGenreID",
                table: "SongStyles",
                newName: "SongStyleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SongStyleID",
                table: "SongStyles",
                newName: "TrackGenreID");
        }
    }
}
