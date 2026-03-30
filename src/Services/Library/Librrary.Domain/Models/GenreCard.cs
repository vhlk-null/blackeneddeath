namespace Library.Domain.Models;

public class GenreCard : Aggregate<GenreCardId>
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? CoverUrl { get; private set; }
    public int OrderNumber { get; private set; }

    private readonly List<GenreCardGenre> _genreCardGenres = [];
    private readonly List<GenreCardTag> _genreCardTags = [];

    public IReadOnlyList<GenreCardGenre> GenreCardGenres => _genreCardGenres.AsReadOnly();
    public IReadOnlyList<GenreCardTag> GenreCardTags => _genreCardTags.AsReadOnly();

    private GenreCard() { }

    public static GenreCard Create(GenreCardId id, string name, string description, string? coverUrl = null, int orderNumber = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var card = new GenreCard
        {
            Id = id,
            Name = name,
            Description = description,
            CoverUrl = coverUrl,
            OrderNumber = orderNumber
        };

        card.AddDomainEvent(new GenreCardCreatedEvent(card));

        return card;
    }

    public GenreCard Update(string name, string description, string? coverUrl, int orderNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Name = name;
        Description = description;
        CoverUrl = coverUrl;
        OrderNumber = orderNumber;

        AddDomainEvent(new GenreCardUpdatedEvent(this));

        return this;
    }

    public void AddGenre(GenreId genreId)
    {
        if (_genreCardGenres.Any(g => g.GenreId == genreId))
            throw new DomainException($"Genre {genreId.Value} is already on this card.");

        _genreCardGenres.Add(new GenreCardGenre(Id, genreId));
    }

    public void RemoveGenre(GenreId genreId)
    {
        var item = _genreCardGenres.FirstOrDefault(g => g.GenreId == genreId)
            ?? throw new DomainException($"Genre {genreId.Value} is not on this card.");

        _genreCardGenres.Remove(item);
    }

    public void AddTag(TagId tagId)
    {
        if (_genreCardTags.Any(t => t.TagId == tagId))
            throw new DomainException($"Tag {tagId.Value} is already on this card.");

        _genreCardTags.Add(new GenreCardTag(Id, tagId));
    }

    public void RemoveTag(TagId tagId)
    {
        var item = _genreCardTags.FirstOrDefault(t => t.TagId == tagId)
            ?? throw new DomainException($"Tag {tagId.Value} is not on this card.");

        _genreCardTags.Remove(item);
    }

    public void Remove()
    {
        AddDomainEvent(new GenreCardRemovedEvent(this));
    }
}
