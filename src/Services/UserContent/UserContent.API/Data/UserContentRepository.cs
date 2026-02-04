namespace UserContent.API.Data
{
    public class UserContentRepository : BaseGenericRepository<UserContentContext>
    {
        public UserContentRepository(UserContentContext context)
        {
            Context = context;
        }
    }
}