using MusicCollection.Models.Entities.Base;

namespace MusicCollection.Models.Entities;

public class AlbumImage : BaseEntity
{    
    public byte[] Data { get; set; } = null!;
    public int AlbumId { get; set; }
    public Album Album { get; set; } = null!;
}