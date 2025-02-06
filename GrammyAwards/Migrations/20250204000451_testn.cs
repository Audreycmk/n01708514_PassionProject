using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammyAwards.Migrations
{
    /// <inheritdoc />
    public partial class testn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArtistName",
                table: "Artists",
                newName: "Artistname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Artistname",
                table: "Artists",
                newName: "ArtistName");
        }
    }
}
