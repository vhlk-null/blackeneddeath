using BuildingBlocks.Repositories;

namespace UserContent.API.Data
{
    public class UserConentRepository : BaseGenericRepository<UserContentContext>
    {
        public UserConentRepository(UserContentContext context)
        {
            Context = context;
        }
    }
}