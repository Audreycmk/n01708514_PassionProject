using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongAwardController : ControllerBase
    {
        private readonly ISongAwardService _songAwardService;

        public SongAwardController(ISongAwardService songAwardService)
        {
            _songAwardService = songAwardService;
        }

        [HttpGet("GetSongsByAward/{awardId}")]
        public async Task<ActionResult<IEnumerable<SongAwardDto>>> GetSongsByAward(int awardId)
        {
            var songAwards = await _songAwardService.GetSongsByAward(awardId);
            return Ok(songAwards);
        }

        [HttpGet("GetAwardsBySong/{songId}")]
        public async Task<ActionResult<IEnumerable<SongAwardDto>>> GetAwardsBySong(int songId)
        {
            var songAwards = await _songAwardService.GetAwardsBySong(songId);
            return Ok(songAwards);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<SongAwardDto>> AddSongAward(SongAwardDto songAwardDto)
        {
            var response = await _songAwardService.AddSongAward(songAwardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }
            return CreatedAtAction(nameof(GetSongsByAward), new { awardId = response.CreatedId }, songAwardDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateSongAward(int songAwardId, SongAwardDto songAwardDto)
        {
            if (songAwardId != songAwardDto.SongAwardId)
            {
                return BadRequest("ID mismatch.");
            }

            var response = await _songAwardService.UpdateSongAward(songAwardId, songAwardDto);
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

        [HttpDelete("UnlinkSongFromAward/{songId}/{awardId}")]
    public async Task<ActionResult<ServiceResponse>> UnlinkSongFromAward(int songId, int awardId)
    {
        var response = await _songAwardService.UnlinkSongFromAward(songId, awardId);

        if (response.Status == ServiceResponse.ServiceStatus.NotFound)
        {
            return NotFound(response); // 404 Not Found
        }

        if (response.Status == ServiceResponse.ServiceStatus.Error)
        {
            return BadRequest(response); // 400 Bad Request
        }

        return Ok(response); // 200 OK if unlinking was successful
    }

        [HttpDelete("{songAwardId}")]
        public async Task<ActionResult> DeleteSongAward(int songAwardId)
        {
            var response = await _songAwardService.DeleteSongAward(songAwardId);
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
