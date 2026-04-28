namespace MusicCollection.Models.ViewModels;

public class AlbumGridVm
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public byte[]? CoverData { get; set; } // Оригинал из БД для последующей конвертации в UI
}
