namespace Library.Domain.Models;

public class Track : Entity<TrackId>
{
    public string Title { get; private set; }

    internal Track(string title)
    {
        Title = title;
    }
}
