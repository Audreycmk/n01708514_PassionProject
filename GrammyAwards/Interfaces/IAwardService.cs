using System.Collections.Generic;
using System.Threading.Tasks;
using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface IAwardService
    {
        Task<IEnumerable<AwardDto>> List();
        Task<AwardDto> GetAwardById(int awardId);
        Task<ServiceResponse<AwardDto>> AddAward(AwardDto awardDto);
        Task<ServiceResponse> UpdateAward(int awardId, AwardDto awardDto);
        Task<ServiceResponse> DeleteAward(int awardId);
    }
}