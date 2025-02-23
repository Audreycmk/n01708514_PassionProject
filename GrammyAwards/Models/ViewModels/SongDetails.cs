namespace GrammyAwards.Models.ViewModels
{
public class SongDetails

{
    public SongDto Song { get; set; }
    public IEnumerable<SongArtistDto> ArtistSongs { get; set; }
    //Add this special Dto !
    public IEnumerable<GetSongListDto> SongAwards { get; set; }
}
}