using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Models;
using GrammyAwards.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject ApplicationDbContext into the controller
        public ArtistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all artists.
        /// </summary>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists()
        {
            var artists = await _context.Artists.ToListAsync();
            var artistDtos = new List<ArtistDto>();

            foreach (var artist in artists)
            {
                int songCount = await _context.SongArtists
                    .Where(sa => sa.ArtistId == artist.ArtistId)
                    .CountAsync();

                artistDtos.Add(new ArtistDto
                {
                    ArtistId = artist.ArtistId,
                    ArtistName = artist.ArtistName,
                    Nationality = artist.Nationality,
                    NumberOfSongs = songCount
                });
            }
            return Ok(artistDtos);
        }

        /// <summary>
        /// Returns a single artist by ID.
        /// </summary>
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<ArtistDto>> FindArtist(int id)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(e => e.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            int numberOfSongs = await _context.SongArtists.CountAsync(sa => sa.ArtistId == id);

            return Ok(new ArtistDto
            {
                ArtistId = artist.ArtistId,
                ArtistName = artist.ArtistName,
                Nationality = artist.Nationality,
                NumberOfSongs = numberOfSongs
            });
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        [HttpPost("Add")]
        public async Task<ActionResult<ArtistDto>> AddArtist(ArtistDto artistDto)
        {
            // Ensure we create the artist correctly
            var artist = new Artist
            {
                ArtistName = artistDto.ArtistName,
                Nationality = artistDto.Nationality
            };

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindArtist), new { id = artist.ArtistId }, new ArtistDto
            {
                ArtistId = artist.ArtistId,
                ArtistName = artist.ArtistName,
                Nationality = artist.ArtistName
            });
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateArtist(int id, ArtistDto artistDto)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            // Update artist properties
            artist.ArtistName = artistDto.ArtistName;
            artist.Nationality = artistDto.Nationality;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating artist.");
            }
        }

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting artist.");
            }
        }

        /// <summary>
        /// Creates a new artist (MVC version for UI).
        /// </summary>
        // [HttpPost("Create")]
        // public async Task<IActionResult> Create(ArtistDto artistDto)
        // {
        //     Console.WriteLine("➡️ Create in ArtistController called");

        //     if (!ModelState.IsValid)
        //     {
        //         Console.WriteLine("ModelState Invalid");
        //         return BadRequest(ModelState);
        //     }

        //     // Create a new artist object
        //     var artist = new Artist
        //     {
        //         ArtistName = artistDto.ArtistName,
        //         Nationality = artistDto.Nationality
        //     };

        //     _context.Artists.Add(artist); 
        //     await _context.SaveChangesAsync();

        //     Console.WriteLine($"Artist Created: {artist.ArtistId}");

        //     return RedirectToAction(nameof(FindArtist), new { id = artist.ArtistId });
        // }
    }
}
