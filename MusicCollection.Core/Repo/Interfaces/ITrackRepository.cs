using MusicCollection.Core.Repo.Base;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo.Interfaces;

public interface ITrackRepository : IRepository<Track> 
{
    Task<List<Track>> SearchByTitleAsync(string title);
}
