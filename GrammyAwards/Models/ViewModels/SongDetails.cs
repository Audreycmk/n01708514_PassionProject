namespace GrammyAwards.Models.ViewModels
{
    public class SongDetails
    {
        // Represents the artist associated with the song
        public required ArtistDto Artist { get; set; }

        // Represents the list of songs that the artist is involved with (if applicable)
        public IEnumerable<SongDto>? Songs { get; set; }

        // Represents the roles of artists in the song (Primary Artist, Featured Artist, etc.)
        public required IEnumerable<SongArtistDto> ArtistSongs { get; set; }
    }
}
