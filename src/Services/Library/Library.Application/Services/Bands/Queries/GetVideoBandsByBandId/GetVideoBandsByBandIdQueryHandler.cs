namespace Library.Application.Services.Bands.Queries.GetVideoBandsByBandId;

public class GetVideoBandsByBandIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetVideoBandsByBandIdQuery, GetVideoBandsByBandIdResult>
{
    public async ValueTask<GetVideoBandsByBandIdResult> Handle(GetVideoBandsByBandIdQuery query, CancellationToken cancellationToken)
    {
        var bandId = BandId.Of(query.BandId);

        var bandExists = await context.Bands.AnyAsync(b => b.Id == bandId, cancellationToken);
        if (!bandExists)
            throw new BandNotFoundException(query.BandId);

        var videoBands = await context.VideoBands
            .AsNoTracking()
            .Where(vb => vb.BandId == bandId)
            .OrderByDescending(vb => vb.Year)
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

        return new GetVideoBandsByBandIdResult(videoBands);
    }
}
