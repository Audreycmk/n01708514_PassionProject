using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrammyAwards.Models
{
    public class SongArtist
    {
        [Key]
        public int SongArtistId { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
        public string SongName { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public string ArtistName { get; set; }
        public string Role { get; set; }
    }

    public class SongArtistDto
    {
        public int SongArtistId { get; set; }
        public int SongId { get; set; }
        public string SongName { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string Role { get; set; }
    }
}
