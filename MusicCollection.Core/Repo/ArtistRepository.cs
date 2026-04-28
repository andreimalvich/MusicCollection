using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Base;
using MusicCollection.Core.Repo.Interfaces;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo;

internal class ArtistRepository : BaseRepository<Artist>, IArtistRepository
{

    public ArtistRepository(ApplicationDbContext context) : base(context)
    {        
    }

    public async Task<List<Artist>> GetAlphabeticalAsync()
    {
        return await Table
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync();
    }
}
