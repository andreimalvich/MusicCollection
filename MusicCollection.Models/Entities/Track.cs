using MusicCollection.Models.Entities.Base;

namespace MusicCollection.Models.Entities;

public class Track : BaseEntity
{    
    public int Number { get; set; } // Номер на диске
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int PhysicalDiscId { get; set; }
    public PhysicalDisc PhysicalDisc { get; set; } = null!;
}
