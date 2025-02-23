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

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<ArtistDto?> artistDtos = await _artistService.List();
            return View(artistDtos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var artistDetails = await _artistService.FindArtist(id);
            if (artistDetails == null)
            {
                return NotFound();
            }

            var songArtistDtos = await _songArtistService. GetSongsByArtist(id);

            var artistViewModel = new ArtistDetails
            {
                Artist = artistDetails,
            
            };

            return View(artistViewModel);
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var artistDetails = await _artistService.FindArtist(id);
            if (artistDetails == null)
            {
                return View("Error");
            }

            var songArtistDtos = await _songArtistService.GetSongsByArtist(id);

            var artistViewModel = new ArtistDetails
            {
                Artist = artistDetails,
    
            };

            return View(artistViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.UpdateArtist(id, artistDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "ArtistPage", new { id = id });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ArtistDto? artistDto = await _artistService.FindArtist(id);
            if (artistDto == null)
            {
                return View("Error");
            }
            return View(artistDto);
        }

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
