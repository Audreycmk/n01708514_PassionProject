
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrammyAwards.Models
{
    public class SongAward
    {
        [Key]
        public int SongAwardId { get; set; } 
        [ForeignKey("Song")]
        public int SongId { get; set; } 
        public virtual Song Song { get; set; } = null!;
        [ForeignKey("Award")]
        public int AwardId { get; set; }
        public string AwardName { get; set; }
        public virtual Award Award { get; set; } = null!;

        public string AwardStatus { get; set; }
    }
public class GetSongAwardDto //for GetSongsByAward
    {
        public int SongAwardId { get; set; } 
        // public int AwardId { get; set;}
        // public string AwardName { get; set; }
        public int SongId { get; set; } 
        public string SongName { get; set; }
        public string AwardStatus { get; set;}
    }

public class GetSongListDto //for GetAwardsBySong
    {
        // public int SongAwardId { get; set; } 
        // public int SongId { get; set; } 
        // public string SongName { get; set; }
        public int AwardId { get; set;}
        public string AwardName { get; set; }
        public string AwardStatus { get; set;}
    }
public class SongAwardDto //for add + update
    {
        public int SongAwardId { get; set; } 
        public int AwardId { get; set;}
        public int SongId { get; set; } 
        public string AwardStatus { get; set;}

    }

}