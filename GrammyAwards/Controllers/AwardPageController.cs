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
        private readonly ISongAwardService _songAwardService;

        public AwardPageController(IAwardService awardService, ISongAwardService songAwardService)
        {
            _awardService = awardService;
            _songAwardService = songAwardService;
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
    // Get the award DTO from the service
    AwardDto awardDto = await _awardService.GetAwardById(id);
    if (awardDto == null)
    {
        return NotFound();
    }

    // Map the AwardDto to an AwardDetails view model
    var awardDetails = new AwardDetails
    {
        Award = awardDto,
        AwardSongs = await _songAwardService.GetSongsByAward(awardDto.AwardId)
    };

    return View(awardDetails);
}



        // GET: AwardPage/New
        public IActionResult New()
        {
            return View();
        }

        // POST: AwardPage/New
        [HttpPost]
        public async Task<IActionResult> New(AwardDto awardDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _awardService.AddAward(awardDto);
                if (response.Status == ServiceResponse.ServiceStatus.Created)
                {
                    return RedirectToAction("Index");
                }
                // Handle errors
                return View("Error", response.Messages);
            }
            return View(awardDto);
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


        // GET: AwardPage/Delete/1
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var awardDto = await _awardService.GetAwardById(id);
            if (awardDto == null)
            {
                return NotFound();
            }
            return View(awardDto);
        }

        // POST: AwardPage/Delete/1
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _awardService.DeleteAward(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("Index");
            }
            return View("Error", response.Messages);
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
