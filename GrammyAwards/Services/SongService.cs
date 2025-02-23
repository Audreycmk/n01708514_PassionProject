using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Data;

namespace GrammyAwards.Services
{

    public class SongService : ISongService
{
    private readonly ApplicationDbContext _context;

    public SongService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SongDto>> List()
    {
        return await _context.Songs
            .Select(song => new SongDto
            {
                SongId = song.SongId,
                SongName = song.SongName,
                Album = song.Album,
                ReleaseYear = song.ReleaseYear
            })
            .ToListAsync();
    }

    public async Task<SongDto?> FindSong(int id)
    {
        var song = await _context.Songs.FirstOrDefaultAsync(e => e.SongId == id);

    // Return null if the song is not found
    if (song == null)
    {
        return null;
    }

    // Return the song details as SongDto
    return new SongDto
            {
                SongId = song.SongId,
                SongName = song.SongName,
                Album = song.Album,
                ReleaseYear = song.ReleaseYear
            };
    }

    public async Task<ServiceResponse> UpdateSong(int id, SongDto songDto)
    {
        var response = new ServiceResponse();

        if (string.IsNullOrWhiteSpace(songDto.SongName))
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("Song name is required.");
            return response;
        }

        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            response.Status = ServiceResponse.ServiceStatus.NotFound;
            response.Messages.Add("Song not found.");
            return response;
        }

        song.SongName = songDto.SongName;
        song.Album = songDto.Album;
        song.ReleaseYear = songDto.ReleaseYear;

        try
        {
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Updated;
        }
        catch (DbUpdateConcurrencyException)
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("Updating song failed.");
        }

        return response;
    }

    public async Task<ServiceResponse> AddSong(SongDto songDto)
    {
        var response = new ServiceResponse();

        if (string.IsNullOrWhiteSpace(songDto.SongName))
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("Song name is required.");
            return response;
        }

        var song = new Song
        {
            SongName = songDto.SongName,
            Album = songDto.Album,
            ReleaseYear = songDto.ReleaseYear
        };

        try
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Created;
            response.CreatedId = song.SongId;
        }
        catch (Exception ex)
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("There was an error adding the song.");
            response.Messages.Add(ex.Message);
        }

        return response;
    }

    public async Task<ServiceResponse> DeleteSong(int id)
    {
        var response = new ServiceResponse();
        var song = await _context.Songs.FindAsync(id);

        if (song == null)
        {
            response.Status = ServiceResponse.ServiceStatus.NotFound;
            response.Messages.Add("Song cannot be deleted because it does not exist.");
            return response;
        }

        try
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Deleted;
        }
        catch (Exception)
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("Error encountered while deleting the song.");
        }

        return response;
    }

    // New method to get songs for a specific artist
    public async Task<IEnumerable<string>> GetSongsForArtist(int artistId)
{
    return await _context.SongArtists
        .Where(sa => sa.ArtistId == artistId)  // Filter by artistId
        .Include(sa => sa.Song)  // Include related Song data
        .Select(sa => sa.Song.SongName)  // Only select SongName
        .ToListAsync();
}

}

}