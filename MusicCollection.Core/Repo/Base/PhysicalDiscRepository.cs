using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Interfaces;
using MusicCollection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicCollection.Core.Repo.Base;

internal class PhysicalDiscRepository : BaseRepository<PhysicalDisc>, IPhysicalDiscRepository
{
    protected PhysicalDiscRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<PhysicalDisc>> GetByFormatAsync(Format format)
    {        
        return await Table
            .Include(d => d.Album)
            .Where(d => d.Album.Packaging == format)
            .ToListAsync();
    }
}
