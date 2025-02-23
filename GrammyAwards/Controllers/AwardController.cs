using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardController : ControllerBase
    {
        private readonly IAwardService _awardService;

        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<AwardDto>>> List()
        {
            var awards = await _awardService.List();
            return Ok(awards);
        }

        [HttpGet("Find/{id}")]
        public async Task<ActionResult<AwardDto>> GetAwardById(int awardId)
        {
            var award = await _awardService.GetAwardById(awardId);
            if (award == null)
            {
                return NotFound();
            }
            return Ok(award);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AwardDto>> AddAward(AwardDto awardDto)
        {
            var response = await _awardService.AddAward(awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }
            return CreatedAtAction(nameof(GetAwardById), new { awardId = response.CreatedId }, awardDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateAward(int awardId, AwardDto awardDto)
        {
            if (awardId != awardDto.AwardId)
            {
                return BadRequest("ID mismatch.");
            }

            var response = await _awardService.UpdateAward(awardId, awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteAward(int awardId)
        {
            var response = await _awardService.DeleteAward(awardId);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }
            return NoContent();
        }
    }
}