namespace MusicCollection.Models.ViewModels;

public class TrackListVm
{
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }

    // Информация о диске для группировки
    public int DiscNumber { get; set; }
    public string? DiscName { get; set; }
}
