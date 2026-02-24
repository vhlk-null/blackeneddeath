namespace Library.Domain.Models.JoinTables;

public class BandCountry : JoinEntity
{
    public CountryId CountryId { get; private set; } = null!;
    public BandId BandId { get; private set; } = null!;

    private BandCountry() { }

    internal BandCountry(CountryId countryId, BandId bandId)
    {
        CountryId = countryId;
        BandId = bandId;
    }
}
