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
            .OrderByDescending(vb => vb.Year)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(vb => new VideoBandDto(
                vb.Id.Value,
                vb.BandId.Value,
                vb.Name,
                vb.Year,
                vb.CountryId != null ? vb.CountryId.Value : null,
                vb.VideoType,
                vb.YoutubeLink,
                vb.Info))
            .ToListAsync(cancellationToken);

        return new GetVideoBandsResult(
            new PaginatedResult<VideoBandDto>(pageIndex, pageSize, totalCount, videoBands));
    }
}
