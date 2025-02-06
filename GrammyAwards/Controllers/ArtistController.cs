using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrammyAwards.Data;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of artists in the system.
        /// </summary>
        /// <example>
        /// GET api/Artist/Get -> [{"artistId":1, "artistName":"Talyor Swift"}]
        /// </example>
        /// <returns>
        /// A list of Artist objects.
        /// </returns>
        [HttpGet(template:"Get")]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _context.Artists.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific artist by ID.
        /// </summary>
        /// <example>
        /// GET api/Artist/Find/1 -> {"artistId":1, "artistName":"Talyor Swift"}
        /// </example>
        /// <returns>
        /// The artist object if found, otherwise NotFound.
        /// </returns>
        [HttpGet(template:"Find/{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        /// <summary>
        /// Adds a new artist to the system.
        /// </summary>
        /// <example>
        /// POST api/Artist/Add 
        /// Body: {"artistName": "/// <summary>
        /// Adds a new artist to the system.
        /// </summary>
        /// <example>
        /// POST api/Artist/Add 
        /// {"artistId": 1,"artistName": "Talyor Swift","nationality": "American"}
        /// </example>
        /// <returns>
        /// The created artist object with its assigned ID.
        /// </returns>"}
        /// </example>
        /// <returns>
        /// The created artist object with its assigned ID.
        /// </returns>
        [HttpPost(template:"Add")]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtist), new { id = artist.ArtistId }, artist);
        }

        // <summary>
        /// Updates an existing artist in the system.
        /// </summary>
        /// <example>
        /// PUT api/Artist/Put/1 
        /// Body: {"artistId": 1, "artistName": "Taylor"}
        /// </example>
        /// <returns>
        /// NoContent if successful, otherwise an error response.
        /// </returns>
        [HttpPut(template:"Put/{id}")]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an artist from the system by ID.
        /// </summary>
        /// <example>
        /// DELETE api/Artist/Delete/1
        /// </example>
        /// <returns>
        /// NoContent if 
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.ArtistId == id);
        }
    }
}