namespace Library.Domain.Models.JoinTables
{
    public class AlbumCountry : JoinEntity
    {
        public AlbumId AlbumId { get; private set; }
        public CountryId CountryId { get; private set; }

        internal AlbumCountry(AlbumId albumId, CountryId countryId)
        {
            AlbumId = albumId;
            CountryId = countryId;
        }
    }
}
