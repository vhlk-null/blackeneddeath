namespace Library.API.Data
{
    public class LibraryRepository : BaseGenericRepository<LibraryContext>
    {
        public LibraryRepository(LibraryContext context)
        {
            Context = context;
        }
    }
}