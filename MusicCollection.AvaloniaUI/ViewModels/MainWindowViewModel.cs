using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MusicCollection.AvaloniaUI.Views;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Repo;
using MusicCollection.Models.Entities;

namespace MusicCollection.AvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly UnitOfWorkFactory _uowFactory;

    public MainWindowViewModel(UnitOfWorkFactory unitOfWorkFactory)
    {
        _uowFactory = unitOfWorkFactory;
        _ = LoadArtistsAsync();
    }

    [ObservableProperty]
    public partial ObservableCollection<Artist> Artists { get; set; } = [];

    [ObservableProperty]
    public partial Artist? SelectedArtist { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Album> Albums { get; set; } = [];

    [ObservableProperty]
    public partial Album? SelectedAlbum { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Track> Tracks { get; set; } = [];

    [ObservableProperty]
    public partial string TotalDuration { get; set; } = "00:00";

    // --- Логика добавления нового альбома ---
    [RelayCommand]
    public async Task OpenAddAlbumDialogAsync(Window owner)
    {
        // 1. Получаем ссылку на главное окно
        var desktop = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var mainWindow = desktop?.MainWindow;
        if (mainWindow == null)
        {
            return;
        }

        // 2. Инициализируем ViewModel (передаем список артистов для ComboBox)
        var vm = new AddAlbumViewModel(Artists);
        var dialog = new AddAlbumWindow
        {
            DataContext = vm,
            Title = "Добавление нового диска",
        };

        // 3. Ждем только закрытия окна
        var result = await dialog.ShowDialog<bool>(mainWindow);

        if (result)
        {
            // ВАЖНО: Как и в редактировании, запускаем сохранение в фоновом потоке,
            // чтобы UI-поток разблокировался немедленно.
            _ = Task.Run(async () => await SaveNewAlbumLogic(vm));
        }
    }

    [RelayCommand]
    public async Task DeleteAlbumAsync(Album album)
    {
        if (album == null)
        {
            return;
        }

        // TODO: Подтверждение удаления - переделать на диалог)
        using var uow = await _uowFactory.CreateAsync();
        uow.Albums.Delete(album);
        await uow.CompleteAsync();
        Albums.Remove(album);
        Tracks.Clear();
    }

    [RelayCommand]
    public async Task EditAlbumAsync(Album album)
    {
        if (album == null)
        {
            return;
        }

        var desktop = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var mainWindow = desktop?.MainWindow;
        if (mainWindow == null)
        {
            return;
        }

        using var uow = await _uowFactory.CreateAsync();
        var fullAlbum = await uow.Albums.GetFullAlbumDetailsAsync(album.Id);
        if (fullAlbum == null)
        {
            return;
        }

        var vm = new AddAlbumViewModel(Artists);
        vm.LoadAlbumData(fullAlbum);

        var dialog = new AddAlbumWindow { DataContext = vm, Title = "Редактирование альбома" };

        // Ждем только результат
        var result = await dialog.ShowDialog<bool>(mainWindow);

        if (result)
        {
            // ВАЖНО: Мы НЕ делаем сохранение прямо здесь.
            // Мы запускаем его отдельно, чтобы UI-поток разблокировался немедленно.
            _ = Task.Run(async () => await SaveEditedAlbumLogic(album.Id, vm));
        }
    }

    [RelayCommand]
    public async Task DeleteArtistAsync(Artist artist)
    {
        if (artist == null)
        {
            return;
        }

        var mainWindow =
            (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?
            .MainWindow as MainWindow;

        if (mainWindow is not null)
        {
            bool confirm = await mainWindow.ConfirmDelete($"Вы уверены, что хотите удалить исполнителя '{SelectedArtist?.Name}' и ВСЕ его альбомы?");

            if (!confirm)
            {
                return;
            }
        }

        // 1. Удаление из базы данных
        using var uow = await _uowFactory.CreateAsync();
        uow.Artists.Delete(artist);
        await uow.CompleteAsync();

        // 2. Обновление интерфейса
        Artists.Remove(artist);

        // Если удаленный артист был выбран — очищаем всё остальное
        if (SelectedArtist == artist)
        {
            SelectedArtist = null;
            Albums.Clear();
            Tracks.Clear();
        }
    }

    [RelayCommand]
    private async Task LoadArtistsAsync()
    {
        using var uow = await _uowFactory.CreateAsync();
        var list = await uow.Artists.GetAlphabeticalAsync();

        Artists = new ObservableCollection<Artist>(list);

        SelectedArtist = null;
        Albums.Clear();
        Tracks.Clear();
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

    private async Task LoadAlbumsAsync(int artistId)
    {
        using var uow = await _uowFactory.CreateAsync();
        var list = await uow.Albums.GetByArtistWithImagesAsync(artistId);

        Albums = new ObservableCollection<Album>(list);
        Tracks.Clear();
    }

    private async Task LoadTracksAsync(int albumId)
    {
        using var uow = await _uowFactory.CreateAsync();
        var album = await uow.Albums.GetFullAlbumDetailsAsync(albumId);

        if (album != null)
        {
            var allTracks = album.Discs
                .OrderBy(d => d.DiscNumber)
                .SelectMany(d => d.Tracks.OrderBy(t => t.Number))
                .ToList();

            Tracks = new ObservableCollection<Track>(allTracks);

            if (!Tracks.Any())
            {
                TotalDuration = "00:00";
                return;
            }

            TimeSpan totalDuration = Tracks.Aggregate(TimeSpan.Zero, (sum, track) => sum + track.Duration);
            TotalDuration = totalDuration.TotalHours >= 1
                ? totalDuration.ToString(@"hh\:mm\:ss")
                : totalDuration.ToString(@"mm\:ss");
        }
    }

    private async Task SaveEditedAlbumLogic(int albumId, AddAlbumViewModel vm)
    {
        using var uow = await _uowFactory.CreateAsync();

        // Загружаем альбом заново в НОВОМ контексте
        var albumToUpdate = await uow.Albums.GetFullAlbumDetailsAsync(albumId);
        if (albumToUpdate == null)
        {
            return;
        }

        // Обновляем поля
        albumToUpdate.Title = vm.Title;
        albumToUpdate.ReleaseYear = (int)vm.ReleaseYear;
        albumToUpdate.Label = vm.Label;
        albumToUpdate.Packaging = vm.SelectedPackaging;

        if (vm.CoverData != null)
        {
            albumToUpdate.Image ??= new AlbumImage();
            albumToUpdate.Image.Data = vm.CoverData;
        }

        // Пересобираем диски и треки
        albumToUpdate.Discs.Clear();
        var tracksByDiscs = vm.NewTracks.GroupBy(t => t.PhysicalDisc.DiscNumber);
        foreach (var group in tracksByDiscs)
        {
            var disc = new PhysicalDisc { DiscNumber = group.Key };
            foreach (var t in group)
            {
                t.PhysicalDisc = disc;
                disc.Tracks.Add(t);
            }

            albumToUpdate.Discs.Add(disc);
        }

        uow.Albums.Update(albumToUpdate);
        await uow.CompleteAsync();

        // Возвращаемся в UI поток ТОЛЬКО для обновления списков
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await LoadAlbumsAsync(albumToUpdate.ArtistId);
        });
    }

    private async Task SaveNewAlbumLogic(AddAlbumViewModel vm)
    {
        // 1. Обработка артиста
        using var uow = await _uowFactory.CreateAsync();
        var artist = vm.SelectedArtist;
        if (artist == null || artist.Name != vm.ArtistNameText)
        {
            var existing = (await uow.Artists.GetAllAsync())
                .FirstOrDefault(a => a.Name.Equals(vm.ArtistNameText, StringComparison.OrdinalIgnoreCase));

            artist = existing ?? new Artist { Name = vm.ArtistNameText };
            if (existing == null)
            {
                await uow.Artists.AddAsync(artist);
                await uow.CompleteAsync();

                // Обновляем список артистов в UI
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(LoadArtistsAsync);
            }
        }

        // 2. Создание альбома
        var newAlbum = new Album
        {
            Title = vm.Title,
            ReleaseYear = (int)vm.ReleaseYear,
            Label = vm.Label,
            CatalogNumber = vm.CatalogNumber,
            Packaging = vm.SelectedPackaging,
            ArtistId = artist.Id,
        };

        if (vm.CoverData != null)
        {
            newAlbum.Image = new AlbumImage { Data = vm.CoverData };
        }

        // 3. Группировка треков
        var tracksByDiscs = vm.NewTracks.GroupBy(t => t.PhysicalDisc.DiscNumber);
        foreach (var group in tracksByDiscs)
        {
            var disc = new PhysicalDisc { DiscNumber = group.Key };
            foreach (var t in group)
            {
                t.PhysicalDisc = disc;
                disc.Tracks.Add(t);
            }

            newAlbum.Discs.Add(disc);
        }

        await uow.Albums.AddAsync(newAlbum);
        await uow.CompleteAsync();

        // 4. Обновляем UI (плитку альбомов)
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (SelectedArtist?.Id == artist.Id)
            {
                await LoadAlbumsAsync(artist.Id);
            }
        });
    }
}
