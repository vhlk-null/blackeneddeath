namespace Library.Domain.Models.JoinTables;

public class AlbumCountry : JoinEntity
{
    public AlbumId AlbumId { get; private set; } = null!;
    public CountryId CountryId { get; private set; } = null!;

    private AlbumCountry() { }

    internal AlbumCountry(AlbumId albumId, CountryId countryId)
    {
        AlbumId = albumId;
        CountryId = countryId;
    }
}
