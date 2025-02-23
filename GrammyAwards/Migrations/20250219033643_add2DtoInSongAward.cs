using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammyAwards.Migrations
{
    /// <inheritdoc />
    public partial class add2DtoInSongAward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwardName",
                table: "SongAwards",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardName",
                table: "SongAwards");
        }
    }
}
