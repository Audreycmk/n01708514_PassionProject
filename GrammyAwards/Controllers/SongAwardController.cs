using GrammyAwards.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Models;

[Route("api/[controller]")]
[ApiController]
public class SongAwardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SongAwardController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of all song-award relationships.
    /// </summary>
    /// <example>
    /// GET api/SongAward/Get -> [{"songAwardId":1, "songId":1, "awardId":1, "awardStatus":"Winner"}]
    /// </example>
    /// <returns>A list of SongAwardDto objects.</returns>
    [HttpGet("Get")]
    public async Task<ActionResult<IEnumerable<SongAwardDto>>> GetSongAwards()
    {
        var songAwards = await _context.SongAwards
            .Select(sa => new SongAwardDto
            {
                SongAwardId = sa.SongAwardId,
                SongId = sa.SongId,
                AwardId = sa.AwardId,
                AwardStatus = sa.AwardStatus
            })
            .ToListAsync();

        return Ok(songAwards);
    }

    /// <summary>
    /// Retrieves a specific song-award relationship by ID.
    /// </summary>
    /// <example>
    /// GET api/SongAward/Find/1 -> {"songAwardId":1, "songId":1, "awardId":1, "awardStatus":"Winner"}
    /// </example>
    /// <returns>The song-award object if found, otherwise NotFound.</returns>
    [HttpGet("Find/{id}")]
    public async Task<ActionResult<SongAwardDto>> GetSongAward(int id)
    {
        var songAward = await _context.SongAwards
            .Where(sa => sa.SongAwardId == id)
            .Select(sa => new SongAwardDto
            {
                SongAwardId = sa.SongAwardId,
                SongId = sa.SongId,
                AwardId = sa.AwardId,
                AwardStatus = sa.AwardStatus
            })
            .FirstOrDefaultAsync();

        if (songAward == null)
        {
            return NotFound();
        }

        return Ok(songAward);
    }

    /// <summary>
    /// Adds a new song-award relationship.
    /// </summary>
    /// <example>
    /// POST api/SongAward/Add
    /// Body: {"songId":1, "awardId":1, "awardStatus":"Winner"}
    /// </example>
    /// <returns>The created SongAwardDto object with its assigned ID.</returns>
    [HttpPost("Add")]
    public async Task<ActionResult<SongAwardDto>> CreateSongAward(SongAwardDto songAwardDto)
    {
        // Map from SongAwardDto to SongAward (the entity)
        var songAward = new SongAward
        {
            SongId = songAwardDto.SongId,
            AwardId = songAwardDto.AwardId,
            AwardStatus = songAwardDto.AwardStatus
        };

        // Add the SongAward entity to the context
        _context.SongAwards.Add(songAward);
        await _context.SaveChangesAsync();

        // Map from SongAward entity back to SongAwardDto for the response
        var createdDto = new SongAwardDto
        {
            SongAwardId = songAward.SongAwardId, // The ID is set after SaveChangesAsync
            SongId = songAward.SongId,
            AwardId = songAward.AwardId,
            AwardStatus = songAward.AwardStatus
        };

        // Return the created DTO with the location of the new resource
        return CreatedAtAction(nameof(GetSongAward), new { id = songAward.SongAwardId }, createdDto);
    }

    // <summary>
    /// Updates an existing song-award relationship.
    /// </summary>
    /// <example>
    /// PUT api/SongAward/Put/1
    /// Body: {"songAwardId":1, "songId":1, "awardId":1, "awardStatus":"Winner"}
    /// </example>
    /// <returns>NoContent if successful, otherwise an error response.</returns>
    [HttpPut("Put/{id}")]
    public async Task<IActionResult> UpdateSongAward(int id, SongAwardDto songAwardDto)
    {
        if (id != songAwardDto.SongAwardId)
        {
            return BadRequest();
        }

        var existingSongAward = await _context.SongAwards.FindAsync(id);
        if (existingSongAward == null)
        {
            return NotFound();
        }

        existingSongAward.SongId = songAwardDto.SongId;
        existingSongAward.AwardId = songAwardDto.AwardId;
        existingSongAward.AwardStatus = songAwardDto.AwardStatus;

        _context.Entry(existingSongAward).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a song-award relationship by ID.
    /// </summary>
    /// <example>
    /// DELETE api/SongAward/Delete/1
    /// </example>
    /// <returns>NoContent if successful, otherwise NotFound.</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSongAward(int id)
    {
        var songAward = await _context.SongAwards.FindAsync(id);
        if (songAward == null)
        {
            return NotFound();
        }

        _context.SongAwards.Remove(songAward);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}