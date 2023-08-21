using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaPlayer.Core.Migrations
{
    /// <inheritdoc />
    public partial class RenamedPropertyLengthDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackLength",
                table: "Tracks",
                newName: "TrackDuration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackDuration",
                table: "Tracks",
                newName: "TrackLength");
        }
    }
}
