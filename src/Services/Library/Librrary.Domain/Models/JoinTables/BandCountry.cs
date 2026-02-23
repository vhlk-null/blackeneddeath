namespace Library.Domain.Models.JoinTables
{
    public class BandCountry : JoinEntity
    {
        public CountryId CountryId { get; private set; }
        public BandId BandId { get; private set; }

        internal BandCountry(CountryId countryId, BandId bandId)
        {
            CountryId = countryId;
            BandId = bandId;
        }
    }
}
