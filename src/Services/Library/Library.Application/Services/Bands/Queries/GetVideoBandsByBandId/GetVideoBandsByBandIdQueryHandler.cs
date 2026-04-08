namespace Library.Application.Services.Bands.Queries.GetVideoBandsByBandId;

public class GetVideoBandsByBandIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetVideoBandsByBandIdQuery, GetVideoBandsByBandIdResult>
{
    public async ValueTask<GetVideoBandsByBandIdResult> Handle(GetVideoBandsByBandIdQuery query, CancellationToken cancellationToken)
    {
        BandId bandId = BandId.Of(query.BandId);

        string bandName = await context.Bands
                              .Where(b => b.Id == bandId)
                              .Select(b => b.Name)
                              .FirstOrDefaultAsync(cancellationToken)
                          ?? throw new BandNotFoundException(query.BandId);

        int pageIndex = query.PaginationRequest.PageIndex;
        int pageSize = query.PaginationRequest.PageSize;

        IQueryable<VideoBand> baseQuery = context.VideoBands
            .AsNoTracking()
            .Where(vb => vb.BandId == bandId);

        long totalCount = await baseQuery.LongCountAsync(cancellationToken);

        List<VideoBandDto> videoBands = await baseQuery
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
