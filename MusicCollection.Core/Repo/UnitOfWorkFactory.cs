using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;

namespace MusicCollection.Core.Repo;

public class UnitOfWorkFactory(IDbContextFactory<ApplicationDbContext> dbFactory)
{
     public async Task<UnitOfWork> CreateAsync()
    {
        var context = await dbFactory.CreateDbContextAsync();
        return new UnitOfWork(context);
    }
}
