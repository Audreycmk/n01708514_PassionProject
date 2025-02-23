using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Models.ViewModels;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    public class SongPageController : Controller
    {
        private readonly ISongService _songService;
        private readonly ISongArtistService _songArtistService;

        public SongPageController(ISongService songService, ISongArtistService songArtistService)
        {
            _songService = songService;
            _songArtistService = songArtistService;
        }


        public IActionResult Index()
            {
                return RedirectToAction("List");
            }

            // List all songs
        public async Task<IActionResult> List()
            {
            // Get all song details (only the SongDto is returned here)
            IEnumerable<SongDto?> songDtos = await _songService.List();
            return View(songDtos);
            }

        // View Song Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Retrieve the song and its artist relationships
            SongDto? songDto = await _songService.FindSong(id);
            var artistSongs = await _songArtistService.GetArtistsBySong(id);

            if (songDto == null)
            {
                return NotFound();
            }

            // Prepare the SongDetails model
            var songDetails = new SongDetails
            {
                Artist = songDto.Artist,
                Songs = new List<SongDto> { songDto }, // If you want to include this song, otherwise just return null.
                ArtistSongs = artistSongs.Select(artist => new SongArtistDto
                {
                    SongId = songDto.SongId,
                    ArtistId = artist.ArtistId,
                    Role = artist.Role
                }).ToList()
            };

            return View(songDetails);
        }

        // Add Artist to Song
        [HttpPost]
        public async Task<IActionResult> AddArtistToSong(int songId, int artistId, string role)
        {
            var songArtistDto = new SongArtistDto
            {
                SongId = songId,
                ArtistId = artistId,
                Role = role
            };

            var response = await _songArtistService.AddSongArtist(songArtistDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = songId });
            }

            // Handle errors (maybe show a message in the view)
            return View("Error", response.Messages);
        }

        // Remove Artist from Song
        [HttpPost]
        public async Task<IActionResult> RemoveArtistFromSong(int artistId, int songId)
        {
            var response = await _songArtistService.UnlinkArtistFromSong(artistId, songId);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("Details", new { id = songId });
            }

            // Handle errors (maybe show a message in the view)
            return View("Error", response.Messages);
        }

        // Update Song's Details (example method)
        [HttpPost]
        public async Task<IActionResult> UpdateSong(int id, SongDto songDto)
        {
            var response = await _songService.UpdateSong(id, songDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", new { id = id });
            }

            // Handle errors (maybe show a message in the view)
            return View("Error", response.Messages);
        }
    }
}
