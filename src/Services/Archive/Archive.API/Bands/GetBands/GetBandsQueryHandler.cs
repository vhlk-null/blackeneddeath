using BuildingBlocks.Extentions;
using BuildingBlocks.Models;

namespace Archive.API.Bands.GetBands
{
    public record GetBandsQuery(int? PageNumber = 1, int? PageSize = 10)
        : IQuery<PagedResult<BandDto>>;

    public class GetBandsQueryHandler(IRepository<ArchiveContext> repo)
        : IQueryHandler<GetBandsQuery, PagedResult<BandDto>>
    {
        public async Task<PagedResult<BandDto>> Handle(
            GetBandsQuery query,
            CancellationToken cancellationToken)
        {
            var bandsQuery = repo.All<Band>()
                .Include(b => b.Country)
                .Include(b => b.Albums).ThenInclude(ab => ab.Album)
                .Include(b => b.Genres).ThenInclude(bg => bg.Genre)
                .OrderBy(b => b.Name)
                .ProjectToType<BandDto>();

            return await bandsQuery.ToPagedResultAsync(
                query.PageNumber ?? 1,
                query.PageSize ?? 10,
                cancellationToken
            );
        }
    }
}
