using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrammyAwards.Controllers
{
    public class ArtistPageController : Controller
    {
        private readonly IArtistService _artistService;
        private readonly ISongService _songService;
        private readonly ISongArtistService _songArtistService;

        public ArtistPageController(IArtistService artistService, ISongService songService, ISongArtistService songArtistService)
        {
            _artistService = artistService;
            _songService = songService;
            _songArtistService = songArtistService;
        }

        // Redirect to List view by default
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // List all artists
        public async Task<IActionResult> List()
        {
            // Get all artist details (only the ArtistDto is returned here)
            IEnumerable<ArtistDto?> artistDtos = await _artistService.List();
            return View(artistDtos);
        }

        // Show artist details along with songs and roles
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ArtistDto? ArtistDto = await _artistService.FindArtist(id);
            IEnumerable<SongArtistDto> SongsAndRoles = await _songArtistService.GetSongsByArtist(id);

            if (ArtistDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find artist"] });
            }
            else
            {
                // information which drives a category page
                ArtistDetails ArtistInfo = new ArtistDetails()
                {
                    Artist = ArtistDto,
                    ArtistSongs = SongsAndRoles
                };
                return View(ArtistInfo);
            }
        }
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.AddArtist(artistDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "ArtistPage", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

    public async Task<IActionResult> Edit(int id)
{
    var artist = await _artistService.FindArtist(id);
    
    if (artist == null)
    {
        return NotFound();
    }

    var artistDto = new ArtistDto
    {
        ArtistId = artist.ArtistId,
        ArtistName = artist.ArtistName,
        Nationality = artist.Nationality
    };

    return View(artistDto);  // Make sure you're passing the correct artistDto
}


[HttpPost]
public async Task<IActionResult> Update(int id, ArtistDto artistDto)
{
    // Update artist with the service
    ServiceResponse response = await _artistService.UpdateArtist(id, artistDto);

    // Check if the update was successful
    if (response.Status == ServiceResponse.ServiceStatus.Updated)
    {
        // Redirect to the Details page with the updated artist ID
        return RedirectToAction("Details", "ArtistPage", new { id = id });
    }

    // If something went wrong, return the error view
    return View("Error", new ErrorViewModel { Errors = response.Messages });
}






        // View for confirming artist deletion
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var artistDetails = await _artistService.FindArtist(id);
            if (artistDetails == null)
            {
                return View("Error");
            }
            return View(artistDetails);
        }

        // Delete artist
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _artistService.DeleteArtist(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ArtistPage");
            }
            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }
    }
}
