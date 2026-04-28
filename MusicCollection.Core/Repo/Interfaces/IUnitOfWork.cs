namespace MusicCollection.Core.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArtistRepository Artists { get; }
    IAlbumRepository Albums { get; }
    IPhysicalDiscRepository Discs { get; }
    ITrackRepository Tracks { get; }

    // Метод для сохранения всех изменений в рамках одной транзакции
    Task<int> CompleteAsync();
}
