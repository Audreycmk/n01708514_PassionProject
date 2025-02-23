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

        public static implicit operator Artist(ArtistDto v)
        {
            throw new NotImplementedException();
        }
    }

    public class ArtistDto
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        public string Nationality { get; set; }    

        public int NumberOfSongs { get; set; } 
    }

}