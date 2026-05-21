using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicCollection.Models.Entities;

namespace MusicCollection.AvaloniaUI.ViewModels;

public partial class AddAlbumViewModel : ViewModelBase
{
    public AddAlbumViewModel()
    {
    }

    public AddAlbumViewModel(IEnumerable<Artist> existingArtists)
    {
        Artists = new ObservableCollection<Artist>(existingArtists);
    }

    // --- Поля Альбома ---
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]

    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial decimal ReleaseYear { get; set; } = DateTime.Now.Year;

    [ObservableProperty]
    public partial string? CatalogNumber { get; set; }

    [ObservableProperty]
    public partial string? Label { get; set; }

    [ObservableProperty]
    public partial Format SelectedPackaging { get; set; } = Format.JewelCase;

    // Список всех артистов для ComboBox
    [ObservableProperty]
    public partial ObservableCollection<Artist> Artists { get; set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial Artist? SelectedArtist { get; set; }

    [ObservableProperty]
    public partial string ArtistNameText { get; set; } = string.Empty;

    // Данные обложки
    [ObservableProperty]
    public partial byte[]? CoverData { get; set; }

    // --- Список треков (наполняем для последующего сохранения) ---
    [ObservableProperty]
    public partial ObservableCollection<Track> NewTracks { get; set; } = [];

    // Вспомогательные свойства для UI
    public IEnumerable<Format> AllPackagings => Enum.GetValues<Format>();

    public bool CanSave => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(ArtistNameText);

    public void LoadAlbumData(Album album)
    {
        Title = album.Title;
        ReleaseYear = album.ReleaseYear;
        Label = album.Label;
        CatalogNumber = album.CatalogNumber;
        SelectedPackaging = album.Packaging;
        SelectedArtist = Artists.FirstOrDefault(a => a.Id == album.ArtistId);

        if (album.Image != null)
        {
            CoverData = album.Image.Data;
        }

        // Загружаем треки (если они были загружены в сущность)
        NewTracks = new ObservableCollection<Track>(
            album.Discs.SelectMany(d => d.Tracks).OrderBy(t => t.Number));
    }

    // Автоматическая синхронизация: если выбрали артиста из списка,
    // обновляем текстовое поле
    partial void OnSelectedArtistChanged(Artist? value)
    {
        if (value != null)
        {
            ArtistNameText = value.Name;
        }
    }

    [RelayCommand]
    private void AddTrack()
    {
        // Создаем трек сразу с объектом диска, чтобы работали привязки в DataGrid
        var track = new Track
        {
            Number = NewTracks.Count + 1,
            Title = "Новое название",
            Duration = TimeSpan.FromMinutes(3),
            PhysicalDisc = new PhysicalDisc
            {
                DiscNumber = 1,
            },
        };

        NewTracks.Add(track);
    }

    [RelayCommand]
    private void RemoveTrack(Track track)
    {
        if (track != null && NewTracks.Contains(track))
        {
            NewTracks.Remove(track);

            // Пересчитываем сквозные номера треков
            for (int i = 0; i < NewTracks.Count; i++)
            {
                NewTracks[i].Number = i + 1;
            }
        }
    }
}
