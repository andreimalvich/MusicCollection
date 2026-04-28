using MusicCollection.Models.Entities.Base;


namespace MusicCollection.Models.Entities;

public class Album : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? CatalogNumber { get; set; }
    public Format Packaging { get; set; } = Format.JewelCase;
    public string? Label { get; set; }

    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;

    public AlbumImage? Image { get; set; }
    public List<PhysicalDisc> Discs { get; set; } = new();
}
