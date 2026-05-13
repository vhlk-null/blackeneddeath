namespace Library.Application.Services.Bands.Queries.GetBandRating;

public class GetBandRatingQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetBandRatingQuery, GetBandRatingResult>
{
    public async ValueTask<GetBandRatingResult> Handle(GetBandRatingQuery query, CancellationToken cancellationToken)
    {
        BandId bandId = BandId.Of(query.BandId);

        int? userRating = await context.BandRatings
            .Where(r => r.UserId == query.UserId && r.BandId == bandId)
            .Select(r => (int?)r.Rating)
            .FirstOrDefaultAsync(cancellationToken);

        double? average = await context.BandRatings
            .Where(r => r.BandId == bandId)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);

        int count = await context.BandRatings
            .CountAsync(r => r.BandId == bandId, cancellationToken);

        return new GetBandRatingResult(query.BandId, userRating, average, count);
    }
}
