using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        /// <summary>
        /// Retrieves a list of all songs.
        /// </summary>
        /// <example>
        /// GET api/Song/Get -> [{"songId":1, "songName":"ME!", "album": "Lover", "releaseYear": "2019"}]
        /// </example>
        /// <returns>A list of SongDto objects.</returns>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongs()
        {
            var songs = await _songService.List();
            return Ok(songs);
        }

        /// <summary>
        /// Retrieves a specific song by ID.
        /// </summary>
        /// <example>
        /// GET api/Song/Find/1 -> {"songId":1, "songName":"ME!", "album": "Lover", "releaseYear": "2019"}
        /// </example>
        /// <returns>The song object if found, otherwise NotFound.</returns>
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<SongDto>> GetSong(int id)
        {
            var song = await _songService.FindSong(id);
            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        /// <summary>
        /// Adds a new song.
        /// </summary>
        /// <example>
        /// POST api/Song/Add
        /// Body: {"songName":"ME!", "album": "Lover", "releaseYear": "2019"}
        /// </example>
        /// <returns>The created SongDto object.</returns>
        [HttpPost("Add")]
        public async Task<ActionResult<SongDto>> AddSong(SongDto songDto)
        {
            var response = await _songService.AddSong(songDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return CreatedAtAction(nameof(GetSong), new { id = response.CreatedId }, songDto);
        }

        /// <summary>
        /// Updates an existing song.
        /// </summary>
        /// <example>
        /// PUT api/Song/Put/1
        /// Body: {"songId":1, "songName":"ME!", "album": "Lover", "releaseYear": "2019"}
        /// </example>
        /// <returns>NoContent if successful, otherwise an error response.</returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutSong(int id, SongDto songDto)
        {
            var response = await _songService.UpdateSong(id, songDto);
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
        /// Deletes a song by ID.
        /// </summary>
        /// <example>
        /// DELETE api/Song/Delete/1
        /// </example>
        /// <returns>NoContent if successful, otherwise NotFound.</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var response = await _songService.DeleteSong(id);
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
