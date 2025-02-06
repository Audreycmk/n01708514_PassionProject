
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
        public virtual Song Song { get; set; }
        [ForeignKey("Award")]
        public int AwardId { get; set; }
        public virtual Award Award { get; set; }

        public required string AwardStatus { get; set; }
    }
public class SongAwardDto
    {
        public int SongAwardId { get; set; } 
        public int SongId { get; set; } 
        public int AwardId { get; set;}

        public required string AwardStatus { get; set;}

    }

}