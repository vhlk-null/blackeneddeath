namespace Library.Domain.ValueObjects;

public record AlbumRelease
{
    public int ReleaseYear { get; init; }
    public AlbumFormat Format { get; init; }

    private AlbumRelease(int releaseYear, AlbumFormat format)
    {
        ReleaseYear = releaseYear;
        Format = format;
    }

    public static AlbumRelease Of(int releaseYear, AlbumFormat format)
    {
        if (releaseYear <= 0)
            throw new DomainException("Release year must be a positive number.");

        return !Enum.IsDefined(format) ? throw new DomainException("Invalid album format.") : new AlbumRelease(releaseYear, format);
    }
}
