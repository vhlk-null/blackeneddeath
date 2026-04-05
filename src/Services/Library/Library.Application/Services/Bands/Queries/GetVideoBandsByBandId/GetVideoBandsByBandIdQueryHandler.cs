namespace Library.Application.Services.Bands.Queries.GetVideoBandsByBandId;

public class GetVideoBandsByBandIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetVideoBandsByBandIdQuery, GetVideoBandsByBandIdResult>
{
    public async ValueTask<GetVideoBandsByBandIdResult> Handle(GetVideoBandsByBandIdQuery query, CancellationToken cancellationToken)
    {
        var bandId = BandId.Of(query.BandId);

        var bandName = await context.Bands
            .Where(b => b.Id == bandId)
            .Select(b => b.Name)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new BandNotFoundException(query.BandId);

        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var baseQuery = context.VideoBands
            .AsNoTracking()
            .Where(vb => vb.BandId == bandId);

        var totalCount = await baseQuery.LongCountAsync(cancellationToken);

        var videoBands = await baseQuery
            .OrderByDescending(vb => vb.Year)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(vb => new VideoBandDto(
                vb.Id.Value,
                vb.BandId.Value,
                bandName,
                vb.Name,
                vb.Year,
                vb.CountryId != null ? vb.CountryId.Value : null,
                vb.VideoType,
                vb.YoutubeLink,
                vb.Info))
            .ToListAsync(cancellationToken);

        return new GetVideoBandsByBandIdResult(
            new PaginatedResult<VideoBandDto>(pageIndex, pageSize, totalCount, videoBands));
    }
}
