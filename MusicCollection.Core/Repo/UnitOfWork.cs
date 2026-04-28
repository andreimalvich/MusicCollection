using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo.Interfaces;

namespace MusicCollection.Core.Repo;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    // Свойства для доступа к конкретным репозиториям
    public IArtistRepository Artists { get; }
    public IAlbumRepository Albums { get; }
    public IPhysicalDiscRepository Discs { get; }
    public ITrackRepository Tracks { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        // Инициализируем репозитории, передавая им общий контекст базы данных
        Artists = new ArtistRepository(_context);
        Albums = new AlbumRepository(_context);
        Discs = new PhysicalDiscRepository(_context);
        Tracks = new TrackRepository(_context);
    }

    /// <summary>
    /// Сохраняет все накопленные изменения в базу данных.
    /// </summary>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Освобождает ресурсы контекста.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
