
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GrammyAwards.Models
{
    public class Award
    {
        [Key]

        public int AwardId { get; set; }

        public string AwardName { get; set; }

        public string Description { get; set; }
 
    }

     public class AwardDto
    {

        public int AwardId { get; set; }

        public string AwardName { get; set; }

        public string Description { get; set; }
    }
}