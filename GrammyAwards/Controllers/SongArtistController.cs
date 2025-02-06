using GrammyAwards.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Models;

[Route("api/[controller]")] // Base route for the controller
[ApiController] // Indicates that this is an API controller
public class SongArtistController : ControllerBase
{
    private readonly ApplicationDbContext _context; // Injected DbContext to interact with the database

    // Constructor to inject the DbContext
    public SongArtistController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns a list of song-artist relationships in the system.
    /// </summary>
    /// <example>
    /// GET api/SongArtist/Get -> [{"songArtistId":1, "songId":1, "artistId":1, "role":" Primary Artist"}]
    /// </example>
    /// <returns>
    /// A list of SongArtistDto objects.
    /// </returns>
    [HttpGet(template:"Get")]
    public async Task<ActionResult<IEnumerable<SongArtistDto>>> GetSongArtists()
    {
        var songArtists = await _context.SongArtists
            .Select(sa => new SongArtistDto
            {
                SongArtistId = sa.SongArtistId,
                SongId = sa.SongId,
                ArtistId = sa.ArtistId,
                Role = sa.Role
            })
            .ToListAsync();

        return Ok(songArtists);
    }

    /// <summary>
    /// Retrieves a specific song-artist relationship by ID.
    /// </summary>
    /// <example>
    /// GET api/SongArtist/Find/1 -> {"songArtistId":1, "songId":1, "artistId":1, "role": "Primary Artist"}
    /// </example>
    /// <returns>
    /// The song-artist object if found, otherwise NotFound.
    /// </returns>
    [HttpGet("Find/{id}")]
    public async Task<ActionResult<SongArtistDto>> GetSongArtist(int id)
    {
        var songArtist = await _context.SongArtists
            .Where(sa => sa.SongArtistId == id)
            .Select(sa => new SongArtistDto
            {
                SongArtistId = sa.SongArtistId,
                SongId = sa.SongId,
                ArtistId = sa.ArtistId,
                Role = sa.Role
            })
            .FirstOrDefaultAsync();

        if (songArtist == null)
        {
            return NotFound();
        }

        return Ok(songArtist);
    }

     /// <summary>
    /// Adds a new song-artist relationship to the system.
    /// </summary>
    /// <example>
    /// POST api/SongArtist/Add 
    /// Body: {"songId":2, "artistId":2, "role":"Producer"}
    /// </example>
    /// <returns>
    /// The created SongArtistDto object with its assigned ID.
    /// </returns>
    [HttpPost("Add")]
    public async Task<ActionResult<SongArtistDto>> CreateSongArtist(SongArtistDto songArtistDto)
    {
        var songArtist = new SongArtist
        {
            SongId = songArtistDto.SongId,
            ArtistId = songArtistDto.ArtistId,
            Role = songArtistDto.Role
        };

        _context.SongArtists.Add(songArtist);
        await _context.SaveChangesAsync(); // Save to database

        var createdDto = new SongArtistDto
        {
            SongArtistId = songArtist.SongArtistId,
            SongId = songArtist.SongId,
            ArtistId = songArtist.ArtistId,
            Role = songArtist.Role
        };

        return CreatedAtAction(nameof(GetSongArtist), new { id = songArtist.SongArtistId }, createdDto);
    }

    /// <summary>
    /// Updates an existing song-artist relationship in the system.
    /// </summary>
    /// <example>
    /// PUT api/SongArtist/Put/1 
    /// Body: {"songArtistId":3, "songId":1, "artistId":1, "role":"Featured Artist"}
    /// </example>
    /// <returns>
    /// NoContent if successful, otherwise an error response.
    /// </returns>
    [HttpPut("Put/{id}")]
    public async Task<IActionResult> UpdateSongArtist(int id, SongArtistDto songArtistDto)
    {
        if (id != songArtistDto.SongArtistId)
        {
            return BadRequest(); // ID mismatch
        }

        var existingSongArtist = await _context.SongArtists.FindAsync(id);
        if (existingSongArtist == null)
        {
            return NotFound(); // Return 404 if not found
        }

        existingSongArtist.SongId = songArtistDto.SongId;
        existingSongArtist.ArtistId = songArtistDto.ArtistId;
        existingSongArtist.Role = songArtistDto.Role;

        _context.Entry(existingSongArtist).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent(); // Return 204 on success
    }

    /// <summary>
    /// Deletes a song-artist relationship from the system by ID.
    /// </summary>
    /// <example>
    /// DELETE api/SongArtist/Delete/1
    /// </example>
    /// <returns>
    /// NoContent if successful, otherwise NotFound.
    /// </returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSongArtist(int id)
    {
        var songArtist = await _context.SongArtists.FindAsync(id);
        if (songArtist == null)
        {
            return NotFound();
        }

        _context.SongArtists.Remove(songArtist);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}