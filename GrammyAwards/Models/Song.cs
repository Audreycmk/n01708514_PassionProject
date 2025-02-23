
using System.ComponentModel.DataAnnotations;

namespace GrammyAwards.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        
        public string SongName { get; set; }

        public string Album { get; set; }

        public int ReleaseYear { get; set; }

        public List<SongArtist> SongArtists { get; set; }
    }

    public class SongDto
    {
        public int SongId { get; set; }
        
        public string SongName { get; set; }

        public string Album { get; set; }

        public int ReleaseYear { get; set; }
        
    }

    public class SongListDto
{
      public int SongId { get; set; }

        public List<string> SongNames { get; set; }
        public int NumberOfSongs { get; internal set; }
}
}