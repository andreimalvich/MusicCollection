using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Base;
using MusicCollection.Core.Repo.Interfaces;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo;

public class PhysicalDiscRepository : BaseRepository<PhysicalDisc>, IPhysicalDiscRepository
{
    public PhysicalDiscRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<PhysicalDisc>> GetByFormatAsync(Format format)
    {
        // Т.к. формат теперь в Альбоме, ищем диски через связь
        return await Table
            .Include(d => d.Album)
            .Where(d => d.Album.Packaging == format)
            .ToListAsync();
    }
}
