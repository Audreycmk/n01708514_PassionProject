using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammyAwards.Migrations
{
    /// <inheritdoc />
    public partial class fromArtistListToArtistNameInSongArtist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtistName",
                table: "SongArtists",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistName",
                table: "SongArtists");
        }
    }
}
