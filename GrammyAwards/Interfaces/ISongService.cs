using System.Collections.Generic;
using System.Threading.Tasks;
using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<SongDto>> List();  // Existing method

        Task<SongDto?> FindSong(int id);  // Existing method

        Task<ServiceResponse> UpdateSong(int id, SongDto songDto);  // Existing method

        Task<ServiceResponse> AddSong(SongDto songDto);  // Existing method

        Task<ServiceResponse> DeleteSong(int id);  // Existing method

        // New method to get songs for a specific artist
        Task<IEnumerable<string>> GetSongsForArtist(int artistId);
    }
}
