using BuildingBlocks.Repositories;

namespace Archive.API.Data
{
    public class ArchiveRepository : BaseGenericRepository<ArchiveContext>
    {
        public ArchiveRepository(ArchiveContext context)
        {
            Context = context;
        }
    }
}