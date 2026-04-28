using MusicCollection.Models.Entities.Base;

namespace MusicCollection.Models.Entities;

public class Artist : BaseEntity
{    
    public string Name { get; set; } = string.Empty;    
    public List<Album> Albums { get; set; } = new();
}
