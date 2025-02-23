namespace GrammyAwards.Models.ViewModels
{
   public class ArtistDetails
{
    public required ArtistDto Artist { get; set; }
    public IEnumerable<SongDto>? Songs { get; set; }
    public required IEnumerable<SongArtistDto> ArtistSongs { get; set; }
}

}
