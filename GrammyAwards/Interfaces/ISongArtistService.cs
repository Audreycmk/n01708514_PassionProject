using System.Collections.Generic;
using System.Threading.Tasks;
using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface ISongArtistService
    {
        Task<IEnumerable<SongDto>> GetSongsByArtist(int artistId);
        Task<IEnumerable<ArtistDto>> GetArtistsBySong(int songId);
        Task<ServiceResponse<SongArtistDto>> AddSongArtist(SongArtistDto songArtistDto);
        Task<ServiceResponse> UpdateSongArtist(int id, SongArtistDto songArtistDto);
        Task<ServiceResponse> UnlinkArtistFromSong(int artistId, int songId);
        Task<ServiceResponse> DeleteSongArtist(int id);
    }
}
