using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Base;
using MusicCollection.Core.Repo.Interfaces;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo;

public class TrackRepository : BaseRepository<Track>, ITrackRepository
{
    public TrackRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Track>> SearchByTitleAsync(string title)
    {
        return await Table
            .Where(t => t.Title.Contains(title))
            .Include(t => t.PhysicalDisc)
                .ThenInclude(d => d.Album)
            .ToListAsync();
    }
}
