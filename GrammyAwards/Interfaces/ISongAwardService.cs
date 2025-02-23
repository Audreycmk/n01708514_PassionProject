using System.Collections.Generic;
using System.Threading.Tasks;
using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface ISongAwardService
    {
        Task<IEnumerable<GetSongAwardDto>> GetSongsByAward(int awardId);
        Task<IEnumerable<GetSongListDto>> GetAwardsBySong(int songId);
        Task<ServiceResponse<SongAwardDto>> AddSongAward(SongAwardDto songAwardDto);
        Task<ServiceResponse> UpdateSongAward(int songAwardId, SongAwardDto songAwardDto);
        Task<ServiceResponse> UnlinkSongFromAward(int songId, int awardId);
        Task<ServiceResponse> DeleteSongAward(int songAwardId);
    }
}