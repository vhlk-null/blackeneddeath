namespace Library.Domain.Models;

public class Genre : Aggregate<GenreId>
{
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public GenreId? ParentGenreId { get; private set; }

    private readonly List<GenreId> _subGenreIds = [];
    public IReadOnlyList<GenreId> SubGenreIds => _subGenreIds.AsReadOnly();

    private Genre() { }

    public static Genre Create(GenreId id, string name, GenreId? parentGenreId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var genre = new Genre
        {
            Id = id,
            Name = name,
            Slug = SlugHelper.Generate(name),
            ParentGenreId = parentGenreId
        };

        genre.AddDomainEvent(new GenreCreatedEvent(genre));

        return genre;
    }

    public Genre Update(string name, GenreId? parentGenreId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        ParentGenreId = parentGenreId;

        AddDomainEvent(new GenreUpdatedEvent(this));

        return this;
    }

    public void Remove()
    {
        AddDomainEvent(new GenreRemovedEvent(this));
    }
}