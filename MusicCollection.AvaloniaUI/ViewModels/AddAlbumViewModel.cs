using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicCollection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCollection.AvaloniaUI.ViewModels;

public partial class AddAlbumViewModel : ViewModelBase
{
    // --- Поля Альбома ---
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    private string _title = string.Empty;
    [ObservableProperty] 
    private decimal _releaseYear = DateTime.Now.Year;

    [ObservableProperty] private string? _catalogNumber;
    [ObservableProperty] private string? _label;
    [ObservableProperty] private Format _selectedPackaging = Format.JewelCase;

    // Список всех артистов для ComboBox
    [ObservableProperty] private ObservableCollection<Artist> _artists = new();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    private Artist? _selectedArtist;

    [ObservableProperty] private string _artistNameText = string.Empty;

    // Данные обложки
    [ObservableProperty] private byte[]? _coverData;

    // --- Список треков (наполняем для последующего сохранения) ---
    [ObservableProperty]
    private ObservableCollection<Track> _newTracks = new();

    // Вспомогательные свойства для UI
    public IEnumerable<Format> AllPackagings => Enum.GetValues<Format>();
    public bool CanSave => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(ArtistNameText);

    public AddAlbumViewModel()
    {
    }

    public AddAlbumViewModel(IEnumerable<Artist> existingArtists)
    {
        Artists = new ObservableCollection<Artist>(existingArtists);
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
            PhysicalDisc = new PhysicalDisc { DiscNumber = 1 }
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


    public void LoadAlbumData(Album album)
    {
        Title = album.Title;
        ReleaseYear = album.ReleaseYear;
        Label = album.Label;
        CatalogNumber = album.CatalogNumber;
        SelectedPackaging = album.Packaging;
        SelectedArtist = Artists.FirstOrDefault(a => a.Id == album.ArtistId);

        if (album.Image != null)
            CoverData = album.Image.Data;

        // Загружаем треки (если они были загружены в сущность)
        NewTracks = new ObservableCollection<Track>(
            album.Discs.SelectMany(d => d.Tracks).OrderBy(t => t.Number));
    }


}
