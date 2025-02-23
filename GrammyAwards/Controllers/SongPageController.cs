using Microsoft.AspNetCore.Mvc;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Models.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GrammyAwards.Controllers
{
    public class SongPageController : Controller
{
    private readonly ISongService _songService;
    private readonly IAwardService _awardService;
    private readonly ISongAwardService _songAwardService;

    public SongPageController(ISongService songService, IAwardService awardService, ISongAwardService songAwardService)
    {
        _songService = songService;
        _awardService = awardService;
        _songAwardService = songAwardService;
    }

     // Redirect to List view by default
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

    // List all songs
    public async Task<IActionResult> List()
    {
        IEnumerable<SongDto> songDtos = await _songService.List();
        return View(songDtos);
    }   

[HttpGet]
public async Task<IActionResult> Details(int id)
{
    // Fetch the song details
    SongDto? songDto = await _songService.FindSong(id);

    // Explicitly fetch awards associated with the song
    var songAwards = await _songAwardService.GetAwardsBySong(id);

    // If song not found, return an error view
    if (songDto == null)
    {
        return View("Error", new ErrorViewModel() { Errors = new List<string> { "Could not find song" } });
    }
    else
    {
        // Create the view model to pass to the view
        SongDetails songInfo = new SongDetails()
        {
            Song = songDto,               // SongDto passed to 
            SongAwards = songAwards       // List of awards 
        };

        return View(songInfo); // Return the view with the song details
    }
}



    // New Song Page
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    // Create New Song
    [HttpPost]
    public async Task<IActionResult> Add(SongDto songDto)
      {
            ServiceResponse response = await _songService.AddSong(songDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "SongPage", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

    // Edit Song Page
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var songDto = await _songService.FindSong(id);
        if (songDto == null)
        {
            return NotFound();
        }
        return View(songDto);
    }

    // Update Song Details
    [HttpPost]
    public async Task<IActionResult> Update(int id, SongDto songDto)
    {
    // Update artist with the service
    ServiceResponse response = await _songService.UpdateSong(id, songDto);

    // Check if the update was successful
    if (response.Status == ServiceResponse.ServiceStatus.Updated)
    {
        // Redirect to the Details page with the updated artist ID
       return RedirectToAction("Details", "SongPage", new { id = songDto.SongId }); // Make sure you're passing the correct artist ID
    }

    // If something went wrong, return the error view
    return View("Error", new ErrorViewModel { Errors = response.Messages });
}


    // Confirm Delete Page
[HttpGet]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var songDto = await _songService.FindSong(id);
        if (songDto == null)
        {
            return NotFound();
        }
        return View(songDto);
    }

    // Delete Song
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _songService.DeleteSong(id);
        if (response.Status == ServiceResponse.ServiceStatus.Deleted)
        {
            return RedirectToAction("List");
        }
        return View("Error", response.Messages);
    }
}

}
