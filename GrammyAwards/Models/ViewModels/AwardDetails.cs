namespace GrammyAwards.Models.ViewModels
{
    public class AwardDetails
    {
        public int AwardId { get; set; }
        public string AwardName { get; set; }
        public string Description { get; set; }
        
        public AwardDto Award { get; set; }

        // If you want to display a list of songs associated with the award,
        // use the type returned by your SongAwardService. For example:
        public IEnumerable<GetSongAwardDto> AwardSongs { get; set; }
    }
}
