namespace Library.Domain.Models.JoinTables
{
    public class AlbumGenre : JoinEntity
    {
        public AlbumId AlbumId { get; private set; }
        public GenreId GenreId { get; private set; }
        public bool IsPrimary { get; private set; }

        internal AlbumGenre(AlbumId albumId, GenreId genreId, bool isPrimary)
        {
            AlbumId = albumId;
            GenreId = genreId;
            IsPrimary = isPrimary;
        }
    }
}
