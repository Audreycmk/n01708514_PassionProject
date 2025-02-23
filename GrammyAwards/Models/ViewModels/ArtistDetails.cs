namespace GrammyAwards.Models.ViewModels
{
    // Enum to represent artist roles in songs
    public enum ArtistRole
    {
        PrimaryArtist,
        FeaturedArtist,
        Producer
    }

    public class ArtistDetails
    {
        public Artist Artist { get; set; } // The artist details (name, nationality, etc.)

        // Songs associated with the artist
        public IEnumerable<SongArtist>? Songs { get; set; }

        // All available songs (for selection)
        public IEnumerable<SongDto>? AllSongs { get; set; }

        // All nominations or awards the artist has been associated with
        public IEnumerable<AwardDto>? ArtistAwards { get; set; }
    }

    // SongArtist class to represent a song and the artist's role in that song
    public class SongArtist
    {
        public string SongName { get; set; } // Song title
        public string Role { get; set; } // Artist's role in the song (e.g., Primary Artist, Featured Artist, Producer)
    }

    // DTO for Song details
    public class SongDto
    {
        public int SongId { get; set; }
        public string SongName { get; set; }
        public int ReleaseYear { get; set; } // Assuming release year is important
        // Add any other properties as needed
    }

    // DTO for Award details
    public class AwardDto
    {
        public int AwardId { get; set; }
        public string AwardName { get; set; }
        public string AwardCategory { get; set; } // Added to show the category of the award (e.g., Best Song, Best Album, etc.)
        // Add other properties as needed
    }

    // public class SongRole
    // {
    //     public string SongName { get; set; }
    //     public string Role { get; set; }
    // }
}
