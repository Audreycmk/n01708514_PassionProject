using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using System.Threading.Tasks;
using GrammyAwards.Models.ViewModels;

namespace GrammyAwards.Controllers
{
    public class AwardPageController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly ISongService _songService;
        private readonly ISongAwardService _songAwardService;

        public AwardPageController(IAwardService awardService, ISongAwardService songAwardService, ISongService songService)
        {
            _awardService = awardService;
            _songAwardService = songAwardService;
            _songService = songService;
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
            IEnumerable<AwardDto?> artistDtos = await _awardService.List();
            return View(artistDtos);
        }

  [HttpGet]
public async Task<IActionResult> Details(int id)
{
    // Fetch the award details
    AwardDto? awardDto = await _awardService.GetAwardById(id);
    
    // CHANGE => Explicitly fetch songs by award
    var awardAndStatus = await _songAwardService.GetSongsByAward(id);
    
    // If award not found, return an error view
    if (awardDto == null)
    {
        return View("Error", new ErrorViewModel() { Errors = new List<string> { "Could not find award" } });
    }
    else
    {
        // Create the view model to pass to the view
        AwardDetails awardInfo = new AwardDetails()
        {
            Award = awardDto,               // AwardDto passed to Award property
            AwardSongs = awardAndStatus     // List of songs associated with the award
        };

        return View(awardInfo); // Return the view with the award details
    }
}





        // GET: AwardPage/New
        public IActionResult New()
        {
            return View();
        }

        // POST: AwardPage/New
        [HttpPost]
        public async Task<IActionResult> Add(AwardDto awardDto)
        {
            ServiceResponse response = await _awardService.AddAward(awardDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "AwardPage", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }


        // POST: AwardPage/Edit/1
         public async Task<IActionResult> Edit(int id)
{
            var award = await _awardService.GetAwardById(id);
            
            if (award == null)
            {
                return NotFound();
            }

            var awardDto = new AwardDto
            {
                
                AwardId = award.AwardId,
                AwardName = award.AwardName,
                Description = award.Description
            };

            return View(awardDto);  // Make sure you're passing the correct artistDto 
        }

[HttpPost]
public async Task<IActionResult> Update(AwardDto awardDto)
{
    if (ModelState.IsValid)
    {
        var response = await _awardService.UpdateAward(awardDto.AwardId, awardDto);

        if (response.Status == ServiceResponse.ServiceStatus.Updated)
        {
            //This is the redirect to the details page
            return RedirectToAction("Details", new { id = awardDto.AwardId });
        }
        else
        {
            ModelState.AddModelError("", "There was an error updating the award.");
        }
    }

    // If validation failed, return to the edit page
    return View(awardDto);
}



        // Get all SongAwards for an award
        public async Task<IActionResult> SongAwards(int awardId)
        {
            var songAwards = await _songAwardService.GetSongsByAward(awardId);
            return View(songAwards);
        }

        // Link Song to Award
        [HttpPost]
        public async Task<IActionResult> LinkSongToAward(int songId, int awardId, string awardStatus)
        {
            var songAwardDto = new SongAwardDto
            {
                SongId = songId,
                AwardId = awardId,
                AwardStatus = awardStatus
            };

            var response = await _songAwardService.AddSongAward(songAwardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("SongAwards", new { awardId = awardId });
            }
            return View("Error", response.Messages);
        }

        // Unlink Song from Award
        public async Task<IActionResult> UnlinkSongFromAward(int songId, int awardId)
        {
            var response = await _songAwardService.UnlinkSongFromAward(songId, awardId);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("SongAwards", new { awardId = awardId });
            }
            return View("Error", response.Messages);
        }
    }
}
