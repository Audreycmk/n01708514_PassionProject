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
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all songs.
        /// </summary>
        /// <example>
        /// GET api/Song/Get -> [{"songId":1, "songName":"ME!"}]
        /// </example>
        /// <returns>A list of Song objects.</returns>
        [HttpGet(template:"Get")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific song by ID.
        /// </summary>
        /// <example>
        /// GET api/Song/Find/1 -> {"songId":1, "songName":"ME!"}
        /// </example>
        /// <returns>The song object if found, otherwise NotFound.</returns>
        [HttpGet(template:"Find/{id}")] 
        public async Task<ActionResult<Song>> GetSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        /// <summary>
        /// Adds a new song.
        /// </summary>
        /// <example>
        /// POST api/Song/Add
        /// Body: {"songName":"ME!"}
        /// </example>
        /// <returns>The created Song object.</returns>
        [HttpPost(template:"Add")]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSong), new { id = song.SongId }, song);
        }

        /// <summary>
        /// Updates an existing song.
        /// </summary>
        /// <example>
        /// PUT api/Song/Put/1
        /// Body: {"songId":1, "songName":"ME!"}
        /// </example>
        /// <returns>NoContent if successful, otherwise an error response.</returns>
        [HttpPut(template:"Put/{id}")]
        public async Task<IActionResult> PutSong(int id, Song song)
        {
            if (id != song.SongId)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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
        /// Deletes a song by ID.
        /// </summary>
        /// <example>
        /// DELETE api/Song/Delete/1
        /// </example>
        /// <returns>NoContent if successful, otherwise NotFound.</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }
    }
}