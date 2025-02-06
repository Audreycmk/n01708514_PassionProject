using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrammyAwards.Models
{
    public class Artist 
    {
        [Key]
        public int ArtistId { get; set; }

        public required string ArtistName { get; set; }

        public required string Nationality { get; set; }
    
    }

    public class ArtistDto
    {
        public int ArtistId { get; set; }

        public required string ArtistName { get; set; }

        public required string Nationality { get; set; }
    }
}