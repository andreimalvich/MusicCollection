using MusicCollection.Core.Repo.Base;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo.Interfaces;

public interface IArtistRepository : IRepository<Artist>
{
    Task<List<Artist>> GetAlphabeticalAsync();
}
