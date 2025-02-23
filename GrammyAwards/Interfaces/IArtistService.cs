using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface IArtistService
{

    Task<IEnumerable<ArtistDto>> List();

    Task<ArtistDto?> FindArtist(int id);

    Task<ServiceResponse> UpdateArtist(int artistId, ArtistDto ArtistDto);

    Task<ServiceResponse> AddArtist(ArtistDto ArtistDto);

    Task<ServiceResponse> DeleteArtist(int id);
}
}
