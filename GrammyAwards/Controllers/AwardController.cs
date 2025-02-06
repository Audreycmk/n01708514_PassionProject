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
    public class AwardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AwardController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of awards in the system.
        /// </summary>
        /// <example>
        /// GET api/Award/Get -> [{"awardId":1, "awardName":"Song Of The Year", "description":"Quality song containing both lyrics and melody."}]
        /// </example>
        /// <returns>
        /// A list of Award objects.
        /// </returns>
        [HttpGet(template:"Get")]
        public async Task<ActionResult<IEnumerable<Award>>> GetAwards()
        {
            return await _context.Awards.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific award by ID.
        /// </summary>
        /// <example>
        /// GET api/Award/Find/1 -> {"awardId":1, "awardName":"Song Of The Year", "description":"Quality song containing both lyrics and melody."}]
        /// </example>
        /// <returns>
        /// The award object if found, otherwise NotFound.
        /// </returns>
        [HttpGet(template:"Find/{id}")]
        public async Task<ActionResult<Award>> GetAward(int id)
        {
            var award = await _context.Awards.FindAsync(id);

            if (award == null)
            {
                return NotFound();
            }

            return award;
        }

        /// <summary>
        /// Adds a new award to the system.
        /// </summary>
        /// <example>
        /// POST api/Award/Add 
        /// Body: {"awardId":1, "awardName":"Song Of The Year", "description":"Quality song containing both lyrics and melody."}]
        /// </example>
        /// <returns>
        /// The created award object with its assigned ID.
        /// </returns>
        [HttpPost(template:"Add")]
        public async Task<ActionResult<Award>> PostAward(Award award)
        {
            _context.Awards.Add(award);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAward), new { id = award.AwardId }, award);
        }

        /// <summary>
        /// Updates an existing award in the system.
        /// </summary>
        /// <example>
        /// PUT api/Award/Put/1 
        /// Body: {"awardId":1, "awardName":"Song Of The Year", "description":"Quality song containing both lyrics and melody."}
        /// </example>
        /// <returns>
        /// NoContent if successful, otherwise an error response.
        /// </returns>
        [HttpPut(template:"Put/{id}")]
        public async Task<IActionResult> PutAward(int id, Award award)
        {
            if (id != award.AwardId)
            {
                return BadRequest();
            }

            _context.Entry(award).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardExists(id))
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
        /// Deletes an award from the system by ID.
        /// </summary>
        /// <example>
        /// DELETE api/Award/Delete/1
        /// </example>
        /// <returns>
        /// NoContent if successful, otherwise NotFound.
        /// </returns>
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

        private bool AwardExists(int id)
        {
            return _context.Awards.Any(e => e.AwardId == id);
        }
    }
}
