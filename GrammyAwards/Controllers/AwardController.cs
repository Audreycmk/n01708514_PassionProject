using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Data; // Ensure the correct namespace for ApplicationDbContext
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Inject ApplicationDbContext into the controller
        public AwardController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all awards.
        /// </summary>
        [HttpGet("Get")]
        public ActionResult<IEnumerable<AwardDto>> List()
        {
            var awards = _context.Awards
                .Select(a => new AwardDto
                {
                    AwardId = a.AwardId,
                    AwardName = a.AwardName,
                    Description = a.Description
                })
                .ToList();

            return Ok(awards);
        }

        /// <summary>
        /// Retrieves a specific award by ID.
        /// </summary>
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<AwardDto>> GetAwardById(int id)
        {
            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }

            return Ok(new AwardDto
            {
                AwardId = award.AwardId,
                AwardName = award.AwardName,
                Description = award.Description
            });
        }

        /// <summary>
        /// Adds a new award to the database.
        /// </summary>
        [HttpPost("Add")]
        public async Task<ActionResult<AwardDto>> AddAward(AwardDto awardDto)
        {
            var award = new Award
            {
                AwardName = awardDto.AwardName,
                Description = awardDto.Description
            };

            _context.Awards.Add(award);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAwardById), new { id = award.AwardId }, new AwardDto
            {
                AwardId = award.AwardId,
                AwardName = award.AwardName,
                Description = award.Description
            });
        }

        /// <summary>
        /// Updates an existing award.
        /// </summary>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAward(int id, AwardDto awardDto)
        {
            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }

            // Update award properties
            award.AwardName = awardDto.AwardName;
            award.Description = awardDto.Description;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating award.");
            }
        }

        /// <summary>
        /// Deletes an award by ID.
        /// </summary>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAward(int id)
        {
            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }

            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
