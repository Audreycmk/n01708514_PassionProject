using GrammyAwards.Models;

namespace GrammyAwards.Interfaces
{
    public interface IArtistService
{

    Task<IEnumerable<ArtistDto>> List();

    Task<ArtistDto?> FindArtist(int id);

    Task<ServiceResponse> UpdateArtist(int artistId, ArtistDto ArtistDto);

    Task<ArtistDto> AddArtist(ArtistDto artistDto);

    Task<ServiceResponse> DeleteArtist(int id);
}
}
