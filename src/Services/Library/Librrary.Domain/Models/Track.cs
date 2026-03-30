namespace Library.Domain.Models;

public class Track : Entity<TrackId>
{
    public string Title { get; private set; } = null!;
    public string? Duration { get; private set; }

    private Track() { }

    internal Track(string title, string? duration = null)
    {
        Title = title;
        Duration = duration;
        Id = TrackId.Of(Guid.NewGuid());
    }

    public static Track Create(TrackId id, string title, string? duration = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        return new Track { Id = id, Title = title, Duration = duration };
    }

    public void UpdateTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
    }

    public void UpdateDuration(string? duration) => Duration = duration;
}
