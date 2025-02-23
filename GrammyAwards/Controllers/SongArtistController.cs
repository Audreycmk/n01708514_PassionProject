using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongArtistController : ControllerBase
    {
        private readonly ISongArtistService _songArtistService;

        public SongArtistController(ISongArtistService songArtistService)
        {
            _songArtistService = songArtistService;
        }

        /// <summary>
        /// Returns a list of songs by a specific artist.
        /// </summary>
        /// <param name="artistId">The artist ID</param>
        /// <returns>A list of songs associated with the artist.</returns>
        [HttpGet("GetSongsByArtist/{artistId}")]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsByArtist(int artistId)
        {
            var songs = await _songArtistService.GetSongsByArtist(artistId);

            if (songs == null)
            {
                return NotFound();
            }

            return Ok(songs);
        }

        /// <summary>
        /// Returns a list of artists associated with a specific song.
        /// </summary>
        /// <param name="songId">The song ID</param>
        /// <returns>A list of artists who performed on the song.</returns>
        [HttpGet("GetArtistsBySong/{songId}")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtistsBySong(int songId)
        {
            var artists = await _songArtistService.GetArtistsBySong(songId);

            if (artists == null)
            {
                return NotFound();
            }

            return Ok(artists);
        }

        /// <summary>
        /// Adds a new song-artist relationship.
        /// </summary>
        /// <param name="songArtistDto">The song-artist DTO</param>
        /// <returns>The created song-artist DTO</returns>
        [HttpPost("Add")]
        public async Task<ActionResult<SongArtistDto>> AddSongArtist(SongArtistDto songArtistDto)
        {
            var response = await _songArtistService.AddSongArtist(songArtistDto);

            // If an error occurred, return a conflict response
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }

            // Return the newly created SongArtistDto if successful
            return CreatedAtAction(nameof(AddSongArtist), new { id = response.CreatedId }, songArtistDto);
        }

        /// <summary>
        /// Updates an existing song-artist relationship.
        /// </summary>
        /// <param name="id">The ID of the song-artist relationship to update</param>
        /// <param name="songArtistDto">The updated song-artist DTO</param>
        /// <returns>No content if successful</returns>
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateSongArtist(int id, SongArtistDto songArtistDto)
        {
            if (id != songArtistDto.SongArtistId)
            {
                return BadRequest("ID mismatch.");
            }

            var response = await _songArtistService.UpdateSongArtist(id, songArtistDto);

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

        [HttpDelete("UnlinkArtistFromSong/{artistId}/{songId}")]
        public async Task<ActionResult<ServiceResponse>> UnlinkArtistFromSong(int artistId, int songId)
        {
            var response = await _songArtistService.UnlinkArtistFromSong(artistId, songId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response); // 404 Not Found
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(response); // 400 Bad Request if there was an error
            }

            return Ok(response); // 200 OK if the unlink was successful
        }


        /// <summary>
        /// Deletes a song-artist relationship.
        /// </summary>
        /// <param name="id">The ID of the song-artist relationship to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteSongArtist(int id)
        {
            var response = await _songArtistService.DeleteSongArtist(id);

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
