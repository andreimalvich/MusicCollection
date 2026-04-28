using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Base;
using MusicCollection.Core.Repo.Interfaces;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo;

internal class AlbumRepository : BaseRepository<Album>, IAlbumRepository
{
    public AlbumRepository(ApplicationDbContext context) : base(context) 
    { 
    }

    public async Task<List<Album>> GetAlbumsByArtistAsync(int artistId)
    {
        return await Table
            .AsNoTracking()
            .Where(a => a.ArtistId == artistId)
            .Include(a => a.Image)
            .OrderByDescending(a => a.ReleaseYear)
            .ToListAsync();
    }

    public async Task<Album?> GetFullAlbumDetailsAsync(int albumId)
    {
        return await Table
            .AsNoTracking()
            .Include(a => a.Artist)
            .Include(a => a.Image)
            .Include(a => a.Discs.OrderBy(d => d.DiscNumber))
                .ThenInclude(d => d.Tracks.OrderBy(t => t.Number))
            .FirstOrDefaultAsync(a => a.Id == albumId);
    }
}
