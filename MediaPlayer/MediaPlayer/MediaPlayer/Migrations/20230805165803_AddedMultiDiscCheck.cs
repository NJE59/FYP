using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaPlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedMultiDiscCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMultiDisc",
                table: "Albums",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultiDisc",
                table: "Albums");
        }
    }
}
