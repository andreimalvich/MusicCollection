using MusicCollection.Models.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicCollection.Models.Entities;

public class Track : BaseEntity
{    
    public int Number { get; set; } // Номер на диске
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int PhysicalDiscId { get; set; }
    public PhysicalDisc PhysicalDisc { get; set; } = null!;



    [NotMapped] 
    public string DurationDisplay
    {
        get => Duration.ToString(@"mm\:ss");
        set {
            if (TimeSpan.TryParseExact(value, @"mm\:ss", null, out var result))
            {
                Duration = result;
            }
        }
    }
}
