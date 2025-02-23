using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        /// <summary>
        /// Returns a list of all artists.
        /// </summary>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists()
        {
            var artists = await _artistService.List();
            return Ok(artists);
        }

        /// <summary>
        /// Returns a single artist by ID.
        /// </summary>
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<ArtistDto>> FindArtist(int id)
        {
            var artist = await _artistService.FindArtist(id);
            if (artist == null)
            {
                return NotFound();
            }
            return Ok(artist);
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        [HttpPost("Add")]
        public async Task<ActionResult<ArtistDto>> AddArtist(ArtistDto artistDto)
        {
            var response = await _artistService.AddArtist(artistDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return CreatedAtAction(nameof(FindArtist), new { id = response.CreatedId }, artistDto);
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateArtist(int id, ArtistDto artistDto)
        {
            var response = await _artistService.UpdateArtist(id, artistDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var response = await _artistService.DeleteArtist(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return NoContent();
        }
    }
}
