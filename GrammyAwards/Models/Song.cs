
using System.ComponentModel.DataAnnotations;

namespace GrammyAwards.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        
        public required string SongName { get; set; }

        public required string Album { get; set; }

        public int ReleaseYear { get; set; }

    }

    public class SongDto
    {
        public int SongId { get; set; }
        
        public required string SongName { get; set; }

        public required string Album { get; set; }

        public required string ReleaseYear { get; set; }
    }
}