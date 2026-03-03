namespace Library.Domain.Models;

public class Track : Entity<TrackId>
{
    public string Title { get; private set; } = null!;

    private Track() { }

    internal Track(string title)
    {
        Title = title;
        Id = TrackId.Of(Guid.NewGuid());
    }

    public static Track Create(TrackId id, string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        return new Track { Id = id, Title = title };
    }
}
