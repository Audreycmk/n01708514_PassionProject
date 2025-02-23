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
    public class SongArtistService : ISongArtistService
    {
        private readonly ApplicationDbContext _context;

        public SongArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SongArtistDto>> GetSongsByArtist(int artistId)
        {
            var songs = await _context.SongArtists
                .Where(sa => sa.ArtistId == artistId)
                .Select(sa => new SongArtistDto
                {
                    SongId = sa.SongId,
                    SongName = sa.Song.SongName,
                    Role = sa.Role,
                    // Album = sa.Song.Album,
                    // ReleaseYear = sa.Song.ReleaseYear,
                })
                .ToListAsync();

            return songs;
        }

        public async Task<IEnumerable<ArtistDto>> GetArtistsBySong(int songId)
        {
            var artists = await _context.SongArtists
                .Where(sa => sa.SongId == songId)
                .Select(sa => new ArtistDto
                {
                    ArtistId = sa.ArtistId,
                    ArtistName = sa.Artist.ArtistName,
                    Nationality = sa.Artist.Nationality
                })
                .ToListAsync();

            return artists;
        }

        public async Task<ServiceResponse<SongArtistDto>> AddSongArtist(SongArtistDto songArtistDto)
{
    var response = new ServiceResponse<SongArtistDto>();

    try
    {
        // Validate required fields
        if (songArtistDto.SongId <= 0 || songArtistDto.ArtistId <= 0)
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("Invalid SongId or ArtistId.");
            return response;
        }

        // Check if the song-artist relationship already exists
        bool exists = await _context.SongArtists.AnyAsync(sa =>
            sa.SongId == songArtistDto.SongId && 
            sa.ArtistId == songArtistDto.ArtistId && 
            sa.Role == songArtistDto.Role);

        if (exists)
        {
            response.Status = ServiceResponse.ServiceStatus.Error;
            response.Messages.Add("This artist is already linked to this song with the same role.");
            return response;
        }

        // Create new SongArtist entity
        var songArtist = new SongArtist
        {
            SongId = songArtistDto.SongId,
            ArtistId = songArtistDto.ArtistId,
            Role = songArtistDto.Role
        };

        _context.SongArtists.Add(songArtist);
        await _context.SaveChangesAsync();

        // Set response
        response.Status = ServiceResponse.ServiceStatus.Created;
        response.CreatedId = songArtist.SongArtistId;
        response.Data = songArtistDto;
    }
    catch (Exception ex)
    {
        response.Status = ServiceResponse.ServiceStatus.Error;
        response.Messages.Add("Error adding song-artist relationship.");
        response.Messages.Add(ex.Message);
    }

    return response;
}

        

        public async Task<ServiceResponse> UpdateSongArtist(int id, SongArtistDto songArtistDto)
        {
            var response = new ServiceResponse();

            var songArtist = await _context.SongArtists.FindAsync(id);
            if (songArtist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song-artist relationship not found.");
                return response;
            }

            songArtist.SongId = songArtistDto.SongId;
            songArtist.ArtistId = songArtistDto.ArtistId;
            songArtist.Role = songArtistDto.Role;

            try
            {
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error updating song-artist relationship.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> DeleteSongArtist(int id)
        {
            var response = new ServiceResponse();

            var songArtist = await _context.SongArtists.FindAsync(id);
            if (songArtist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song-artist relationship not found.");
                return response;
            }

            try
            {
                _context.SongArtists.Remove(songArtist);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting song-artist relationship.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> UnlinkArtistFromSong(int artistId, int songId)
        {
            var response = new ServiceResponse();

            var songArtist = await _context.SongArtists
                .FirstOrDefaultAsync(sa => sa.ArtistId == artistId && sa.SongId == songId);

            if (songArtist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song-artist relationship not found.");
                return response;
            }

            try
            {
                _context.SongArtists.Remove(songArtist);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error unlinking artist from song.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }
    }
}
