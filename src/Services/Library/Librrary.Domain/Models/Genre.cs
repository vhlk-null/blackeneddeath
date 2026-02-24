namespace Library.Domain.Models;

public class Genre : Aggregate<GenreId>
{
    public string Name { get; private set; } = null!;
    public GenreId? ParentGenreId { get; private set; }

    private readonly List<GenreId> _subGenreIds = [];
    public IReadOnlyList<GenreId> SubGenreIds => _subGenreIds.AsReadOnly();

    private Genre() { }
}