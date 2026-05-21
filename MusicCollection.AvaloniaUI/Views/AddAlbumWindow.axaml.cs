using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MusicCollection.AvaloniaUI.ViewModels;

namespace MusicCollection.AvaloniaUI;

public partial class AddAlbumWindow : Window
{
    public AddAlbumWindow()
    {
        InitializeComponent();
    }

    private async void SelectImage_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel == null)
        {
            return;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите изображение обложки",
            FileTypeFilter =
            [
                FilePickerFileTypes.ImageAll,
            ],
            AllowMultiple = false,
        });

        if (files.Count > 0)
        {
            using var stream = await files[0].OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            if (DataContext is AddAlbumViewModel vm)
            {
                vm.CoverData = ms.ToArray();
            }
        }
    }

    private void Save_Click(object? sender, RoutedEventArgs e) => Close(true);

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
