namespace Library.Application.Services.Albums.Queries.GetAlbumRating;

public class GetAlbumRatingQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumRatingQuery, GetAlbumRatingResult>
{
    public async ValueTask<GetAlbumRatingResult> Handle(GetAlbumRatingQuery query, CancellationToken cancellationToken)
    {
        AlbumId albumId = AlbumId.Of(query.AlbumId);

        Album album = await context.Albums
                          .AsNoTracking()
                          .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
                      ?? throw new AlbumNotFoundException(query.AlbumId);

        int? userRating = null;
        if (query.UserId.HasValue)
        {
            AlbumRating? rating = await context.AlbumRatings
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == query.UserId.Value && r.AlbumId == albumId, cancellationToken);
            userRating = rating?.Rating;
        }

        return new GetAlbumRatingResult(query.AlbumId, userRating, album.AverageRating, album.RatingsCount);
    }
}
