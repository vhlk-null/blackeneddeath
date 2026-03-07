namespace Library.Infrastructure.Repositories;

public class LibraryRepository : BaseGenericRepository<LibraryContext>
{
    public LibraryRepository(LibraryContext context)
    {
        Context = context;
    }
}
