using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrammyAwards.Interfaces;
using GrammyAwards.Models;
using GrammyAwards.Data;

namespace GrammyAwards.Services
{
    public class AwardService : IAwardService
    {
        private readonly ApplicationDbContext _context;

        public AwardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AwardDto>> List()
        {
            var awards = await _context.Awards
                .Select(a => new AwardDto
                {
                    AwardId = a.AwardId,
                    AwardName = a.AwardName,
                    Description = a.Description
                })
                .ToListAsync();

            return awards;
        }

     public async Task<AwardDto> GetAwardById(int id)
{
    // Fetch the award by its ID
    var award = await _context.Awards.FirstOrDefaultAsync(e => e.AwardId == id);

    // Return null if the award is not found
    if (award == null)
    {
        return null;
    }

    // Return the award details as AwardDto
    return new AwardDto
    {
        AwardId = award.AwardId,
        AwardName = award.AwardName,
        Description = award.Description
    };
}


        public async Task<ServiceResponse<AwardDto>> AddAward(AwardDto awardDto)
        {
            var response = new ServiceResponse<AwardDto>();

            var award = new Award
            {
                AwardName = awardDto.AwardName,
                Description = awardDto.Description
            };

            try
            {
                _context.Awards.Add(award);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = award.AwardId;
                response.Data = awardDto;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error adding award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> UpdateAward(int awardId, AwardDto awardDto)
{
    var response = new ServiceResponse();

    // Find the existing award by ID
    var award = await _context.Awards.FindAsync(awardId);
    if (award == null)
    {
        response.Status = ServiceResponse.ServiceStatus.NotFound;
        response.Messages.Add("Award not found.");
        return response;
    }

    // Update award properties
    award.AwardName = awardDto.AwardName;
    award.Description = awardDto.Description;

    try
    {
        await _context.SaveChangesAsync();
        response.Status = ServiceResponse.ServiceStatus.Updated;
        response.CreatedId = award.AwardId;
    }
    catch (Exception ex)
    {
        response.Status = ServiceResponse.ServiceStatus.Error;
        response.Messages.Add($"Error updating award: {ex.Message}");
    }

    return response;
}


        public async Task<ServiceResponse> DeleteAward(int awardId)
        {
            var response = new ServiceResponse();

            var award = await _context.Awards.FindAsync(awardId);
            if (award == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Award not found.");
                return response;
            }

            try
            {
                _context.Awards.Remove(award);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting award.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }
    }
}