using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicCollection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MusicCollection.AvaloniaUI.ViewModels;

public partial class AddAlbumViewModel : ViewModelBase
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private decimal _releaseYear = DateTime.Now.Year;
    [ObservableProperty] private string? _catalogNumber;
    [ObservableProperty] private string? _label;
    [ObservableProperty] private Format _selectedPackaging = Format.JewelCase;

    // Список всех артистов для ComboBox
    [ObservableProperty] private ObservableCollection<Artist> _artists = new();
    [ObservableProperty] private Artist? _selectedArtist;
    [ObservableProperty] private string _artistNameText = string.Empty;

    // Данные обложки
    [ObservableProperty] private byte[]? _coverData;

    public IEnumerable<Format> AllPackagings => Enum.GetValues<Format>();

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
    private async Task SelectCoverAsync()
    {
        // Логика выбора файла будет вызываться из View (через диалог ОС)
        // В MVVM для этого обычно используют сервисы, но для простоты 
        // мы передадим данные через свойство CoverData
    }
}
