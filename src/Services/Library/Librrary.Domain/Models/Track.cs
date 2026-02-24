namespace Library.Domain.Models;

public class Track : Entity<TrackId>
{
    public string Title { get; private set; }

    private Track() { }

    internal Track(string title)
    {
        Title = title;
        Id = TrackId.Of(Guid.NewGuid());
    }
}
