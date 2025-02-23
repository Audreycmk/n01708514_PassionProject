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

        public  string ArtistName { get; set; }

        public  string Nationality { get; set; }

    }

    public class ArtistDto
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        public string Nationality { get; set; }    

        public int NumberOfSongs { get; set; } 
        public List<SongArtistDto> Songs { get; set; } 
        public string Role { get; set; }
    }

}