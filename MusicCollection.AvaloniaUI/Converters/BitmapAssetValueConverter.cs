using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MusicCollection.AvaloniaUI.Converters;

internal class BitmapAssetValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {        
        if (value is byte[] data && data.Length > 0)
        {
            try
            {
                using var ms = new MemoryStream(data);
                return new Bitmap(ms);
            }
            catch { /* Игнорируем ошибки чтения битых данных */ }
        }
        
        try
        {
            var uri = new Uri("avares://MusicCollection.AvaloniaUI/Assets/no_cover.png");
            return new Bitmap(AssetLoader.Open(uri));
        }
        catch
        {
            // Если даже заглушка не нашлась
            return null;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
