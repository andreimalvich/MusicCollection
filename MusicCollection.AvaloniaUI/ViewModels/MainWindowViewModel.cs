using CommunityToolkit.Mvvm.ComponentModel;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo;
using MusicCollection.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MusicCollection.AvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ApplicationDbContextFactory _dbFactory = new();

    [ObservableProperty]
    private ObservableCollection<Artist> _artists = new();
    [ObservableProperty]
    private ObservableCollection<Album> _albums = new();
    [ObservableProperty]
    private ObservableCollection<Track> _tracks = new();
    [ObservableProperty]
    private Artist? _selectedArtist;

    [ObservableProperty]
    private Album? _selectedAlbum;

    public MainWindowViewModel()
    {
        _ = LoadArtistsAsync();
    }

    partial void OnSelectedArtistChanged(Artist? value)
    {
        if (value != null)
        {
            _ = LoadAlbumsAsync(value.Id);
        }
    }
    partial void OnSelectedAlbumChanged(Album? value)
    {
        if (value != null)
        {
            _ = LoadTracksAsync(value.Id);
        }
    }

    private async Task LoadArtistsAsync()
    {
        using var uow = new UnitOfWork(_dbFactory.CreateDbContext(Array.Empty<string>()));
        var list = await uow.Artists.GetAlphabeticalAsync();
        Artists = new ObservableCollection<Artist>(list);
    }

    private async Task LoadAlbumsAsync(int artistId)
    {
        using var context = _dbFactory.CreateDbContext(Array.Empty<string>());
        using var uow = new UnitOfWork(context);        
        var list = await uow.Albums.GetByArtistWithImagesAsync(artistId);
        Albums = new ObservableCollection<Album>(list);
        Tracks.Clear();
    }

    private async Task LoadTracksAsync(int albumId)
    {
        using var context = _dbFactory.CreateDbContext(Array.Empty<string>());
        using var uow = new UnitOfWork(context);

        var album = await uow.Albums.GetFullAlbumDetailsAsync(albumId);

        if (album != null)
        {
            // Выпрямляем структуру: Диски -> Треки в один список
            var allTracks = album.Discs
                .OrderBy(d => d.DiscNumber)
                .SelectMany(d => d.Tracks.OrderBy(t => t.Number))
                .ToList();

            Tracks = new ObservableCollection<Track>(allTracks);
        }
    }
}
