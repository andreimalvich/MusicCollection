using MusicCollection.Core.Repo.Base;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<List<Album>> GetAlbumsByArtistAsync(int artistId);

    // Метод для получения всей структуры альбома (диски и треки)
    Task<Album?> GetFullAlbumDetailsAsync(int albumId);
}
