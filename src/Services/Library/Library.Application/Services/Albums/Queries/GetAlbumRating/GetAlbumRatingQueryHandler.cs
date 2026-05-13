namespace Library.Application.Services.Albums.Queries.GetAlbumRating;

public class GetAlbumRatingQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumRatingQuery, GetAlbumRatingResult>
{
    public async ValueTask<GetAlbumRatingResult> Handle(GetAlbumRatingQuery query, CancellationToken cancellationToken)
    {
        AlbumId albumId = AlbumId.Of(query.AlbumId);

        int? userRating = await context.AlbumRatings
            .Where(r => r.UserId == query.UserId && r.AlbumId == albumId)
            .Select(r => (int?)r.Rating)
            .FirstOrDefaultAsync(cancellationToken);

        double? average = await context.AlbumRatings
            .Where(r => r.AlbumId == albumId)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);

        int count = await context.AlbumRatings
            .CountAsync(r => r.AlbumId == albumId, cancellationToken);

        return new GetAlbumRatingResult(query.AlbumId, userRating, average, count);
    }
}
