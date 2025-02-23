using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammyAwards.Migrations
{
    /// <inheritdoc />
    public partial class SongNameInSongArtistDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SongName",
                table: "SongArtists",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongName",
                table: "SongArtists");
        }
    }
}
