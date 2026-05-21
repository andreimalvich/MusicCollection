namespace MusicCollection.Models.ViewModels;

public class AlbumDetailsVm
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? CatalogNumber { get; set; }
    public string? Label { get; set; }
    public string Packaging { get; set; } = string.Empty; // Enum в виде строки
    public List<TrackListVm> Tracks { get; set; } = new();
}
