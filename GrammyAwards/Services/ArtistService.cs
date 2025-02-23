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
    public class ArtistService : IArtistService
    {
        private readonly ApplicationDbContext _context;

        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArtistDto>> List()
        {
            var artists = await _context.Artists.ToListAsync();
            var artistDtos = new List<ArtistDto>();

            foreach (var artist in artists)
            {
                int songCount = await _context.SongArtists
                    .Where(sa => sa.ArtistId == artist.ArtistId)
                    .CountAsync();

                artistDtos.Add(new ArtistDto
                {
                    ArtistId = artist.ArtistId,
                    ArtistName = artist.ArtistName,
                    Nationality = artist.Nationality,
                    NumberOfSongs = songCount
                });
            }
            return artistDtos;
        }

        public async Task<ArtistDto?> FindArtist(int id)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(e => e.ArtistId == id);
            if (artist == null)
            {
                return null;
            }

            int numberOfSongs = await _context.SongArtists.CountAsync(sa => sa.ArtistId == id);

            return new ArtistDto
            {
                ArtistId = artist.ArtistId,
                ArtistName = artist.ArtistName,
                Nationality = artist.Nationality,
                NumberOfSongs = numberOfSongs
            };
        }

        public async Task<ServiceResponse> UpdateArtist(int artistId,ArtistDto artistDto)
        {
            var response = new ServiceResponse();

            // Find the existing artist by ID
            var artist = await _context.Artists.FindAsync(artistDto.ArtistId);
            if (artist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Artist not found.");
                return response;
            }

            // Update artist properties
            artist.ArtistName = artistDto.ArtistName;
            artist.Nationality = artistDto.Nationality;

            try
            {
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
                response.CreatedId = artist.ArtistId;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add($"Error updating artist: {ex.Message}");
            }

            return response;
        }

 public async Task<ServiceResponse> AddArtist(ArtistDto artistDto)
        {
            var response = new ServiceResponse();

            if (string.IsNullOrWhiteSpace(artistDto.ArtistName))
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Artist name is required.");
                return response;
            }

            var artist = new Artist
            {
                ArtistName = artistDto.ArtistName,
                Nationality = artistDto.Nationality
            };

            try
            {
                _context.Artists.Add(artist);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = artist.ArtistId;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("There was an error adding the artist.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> DeleteArtist(int id)
        {
            var response = new ServiceResponse();
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Artist cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the artist.");
            }

            return response;
        }
    }
}
