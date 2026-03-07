namespace UserContent.Infrastructure.Repositories;

public class UserContentRepository : BaseGenericRepository<UserContentContext>
{
    public UserContentRepository(UserContentContext context)
    {
        Context = context;
    }
}
