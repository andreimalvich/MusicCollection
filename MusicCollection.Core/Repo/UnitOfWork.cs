using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Interfaces;

namespace MusicCollection.Core.Repo;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private bool _disposed = false;    
    public IArtistRepository Artists { get; } = new ArtistRepository(context);
    public IAlbumRepository Albums { get; } = new AlbumRepository(context);
    public IPhysicalDiscRepository Discs { get; } = new PhysicalDiscRepository(context);
    public ITrackRepository Tracks { get; } = new TrackRepository(context);

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {                
                context?.Dispose();
            }

            // Здесь можно очистить неуправляемые ресурсы (если они когда-то появятся)
            _disposed = true;
        }
    }
}
