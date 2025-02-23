using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Data;

namespace GrammyAwards.Services
{
    public class SongAwardService : ISongAwardService
    {
        private readonly ApplicationDbContext _context;

        public SongAwardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetSongAwardDto>> GetSongsByAward(int awardId)
        {
            var songAwards = await _context.SongAwards
                .Where(sa => sa.AwardId == awardId)
                .Select(sa => new GetSongAwardDto
                 {
                    SongAwardId = sa.SongAwardId,
                    // AwardId = sa.AwardId,
                    // AwardName = sa.Award.AwardName,
                    SongId = sa.SongId,
                    SongName = sa.Song.SongName,
                    AwardStatus = sa.AwardStatus
                })
                .ToListAsync();

            return songAwards;
        }

        public async Task<IEnumerable<GetSongListDto>> GetAwardsBySong(int songId)
        {
            var songAwards = await _context.SongAwards
                .Where(sa => sa.SongId == songId)
                .Select(sa => new GetSongListDto
                {
                    // SongAwardId = sa.SongAwardId,
                    // SongId = sa.SongId,
                    // SongName = sa.Song.SongName,
                    AwardId = sa.AwardId,
                    AwardName = sa.Award.AwardName,
                    AwardStatus = sa.AwardStatus
                })
                .ToListAsync();

            return songAwards;
        }

        public async Task<ServiceResponse<SongAwardDto>> AddSongAward(SongAwardDto songAwardDto)
        {
            var response = new ServiceResponse<SongAwardDto>();
            var songAward = new SongAward
            {
                SongId = songAwardDto.SongId,
                AwardId = songAwardDto.AwardId,
                AwardStatus = songAwardDto.AwardStatus
            };

            try
            {
                _context.SongAwards.Add(songAward);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = songAward.SongAwardId;
                response.Data = songAwardDto;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error adding song award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> UpdateSongAward(int songAwardId, SongAwardDto songAwardDto)
        {
            var response = new ServiceResponse();
            var songAward = await _context.SongAwards.FindAsync(songAwardId);
            if (songAward == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song award not found.");
                return response;
            }

            songAward.SongId = songAwardDto.SongId;
            songAward.AwardId = songAwardDto.AwardId;
            songAward.AwardStatus = songAwardDto.AwardStatus;

            try
            {
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error updating song award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse> UnlinkSongFromAward(int songId, int awardId)
        {
            var response = new ServiceResponse();

            // Find the SongAward entry for the given songId and awardId
            var songAward = await _context.SongAwards
                .FirstOrDefaultAsync(sa => sa.SongId == songId && sa.AwardId == awardId);

            if (songAward == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("The song is not linked to this award.");
                return response;
            }

            try
            {
                // Remove the relationship
                _context.SongAwards.Remove(songAward);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Deleted;
                response.Messages.Add("Song successfully unlinked from the award.");
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error unlinking song from award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }


        public async Task<ServiceResponse> DeleteSongAward(int songAwardId)
        {
            var response = new ServiceResponse();
            var songAward = await _context.SongAwards.FindAsync(songAwardId);
            if (songAward == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song award not found.");
                return response;
            }

            try
            {
                _context.SongAwards.Remove(songAward);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting song award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }
    }
}