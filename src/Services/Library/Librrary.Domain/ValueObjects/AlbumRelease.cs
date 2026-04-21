namespace Library.Domain.ValueObjects;

public record AlbumRelease
{
    public int ReleaseYear { get; init; }
    public int? ReleaseMonth { get; init; }
    public int? ReleaseDay { get; init; }
    public AlbumFormat Format { get; init; }

    private AlbumRelease(int releaseYear, AlbumFormat format, int? releaseMonth, int? releaseDay)
    {
        ReleaseYear = releaseYear;
        Format = format;
        ReleaseMonth = releaseMonth;
        ReleaseDay = releaseDay;
    }

    public static AlbumRelease Of(int releaseYear, AlbumFormat format, int? releaseMonth = null, int? releaseDay = null)
    {
        if (releaseYear <= 0)
            throw new DomainException("Release year must be a positive number.");

        if (!Enum.IsDefined(format))
            throw new DomainException("Invalid album format.");

        if (releaseMonth is < 1 or > 12)
            throw new DomainException("Release month must be between 1 and 12.");

        if (releaseDay is < 1 or > 31)
            throw new DomainException("Release day must be between 1 and 31.");

        return new AlbumRelease(releaseYear, format, releaseMonth, releaseDay);
    }
}
