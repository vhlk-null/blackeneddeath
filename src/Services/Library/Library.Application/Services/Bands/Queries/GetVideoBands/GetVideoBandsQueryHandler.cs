namespace Library.Application.Services.Bands.Queries.GetVideoBands;

public class GetVideoBandsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetVideoBandsQuery, GetVideoBandsResult>
{
    public async ValueTask<GetVideoBandsResult> Handle(GetVideoBandsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var baseQuery = context.VideoBands.AsNoTracking();

        var totalCount = await baseQuery.LongCountAsync(cancellationToken);

        var videoBands = await baseQuery
            .Join(context.Bands, vb => vb.BandId, b => b.Id, (vb, b) => new { vb, BandName = b.Name })
            .OrderByDescending(x => x.vb.Year)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(x => new VideoBandDto(
                x.vb.Id.Value,
                x.vb.BandId.Value,
                x.BandName,
                x.vb.Name,
                x.vb.Year,
                x.vb.CountryId != null ? x.vb.CountryId.Value : null,
                x.vb.VideoType,
                x.vb.YoutubeLink,
                x.vb.Info))
            .ToListAsync(cancellationToken);

        return new GetVideoBandsResult(
            new PaginatedResult<VideoBandDto>(pageIndex, pageSize, totalCount, videoBands));
    }
}
