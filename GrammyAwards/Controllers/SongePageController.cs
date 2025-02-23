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

    public SongPageController(ISongService songService)
    {
        _songService = songService;
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

    // New Song Page
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    // Create New Song
    [HttpPost]
    public async Task<IActionResult> New(SongDto songDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _songService.AddSong(songDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List");
            }
            // Handle errors (maybe show a message in the view)
            return View("Error", response.Messages);
        }
        return View(songDto);
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
    public async Task<IActionResult> Edit(int id, SongDto songDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _songService.UpdateSong(id, songDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List");
            }
            return View("Error", response.Messages);
        }
        return View(songDto);
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
