using BuildingBlocks.Extentions;
using Library.API.Data;
using Library.API.Models;

namespace Library.API.Bands.GetBands
{
    public record GetBandsQuery(int? PageNumber = 1, int? PageSize = 10)
        : IQuery<PagedResult<BandDto>>;

    public class GetBandsQueryHandler(IRepository<LibraryContext> repo)
        : IQueryHandler<GetBandsQuery, PagedResult<BandDto>>
    {
        public async ValueTask<PagedResult<BandDto>> Handle(
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
