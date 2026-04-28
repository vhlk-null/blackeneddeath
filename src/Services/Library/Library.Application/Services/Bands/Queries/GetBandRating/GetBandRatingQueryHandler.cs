namespace Library.Application.Services.Bands.Queries.GetBandRating;

public class GetBandRatingQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetBandRatingQuery, GetBandRatingResult>
{
    public async ValueTask<GetBandRatingResult> Handle(GetBandRatingQuery query, CancellationToken cancellationToken)
    {
        BandId bandId = BandId.Of(query.BandId);

        Band band = await context.Bands
                        .AsNoTracking()
                        .FirstOrDefaultAsync(b => b.Id == bandId, cancellationToken)
                    ?? throw new BandNotFoundException(query.BandId);

        int? userRating = null;
        if (query.UserId.HasValue)
        {
            BandRating? rating = await context.BandRatings
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == query.UserId.Value && r.BandId == bandId, cancellationToken);
            userRating = rating?.Rating;
        }

        return new GetBandRatingResult(query.BandId, userRating, band.AverageRating, band.RatingsCount);
    }
}
