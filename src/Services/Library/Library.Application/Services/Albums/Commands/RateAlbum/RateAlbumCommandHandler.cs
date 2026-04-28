namespace Library.Application.Services.Albums.Commands.RateAlbum;

public class RateAlbumCommandHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<RateAlbumCommand, RateAlbumResult>
{
    public async ValueTask<RateAlbumResult> Handle(RateAlbumCommand command, CancellationToken cancellationToken)
    {
        AlbumId albumId = AlbumId.Of(command.AlbumId);

        Album album = await context.Albums
                          .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
                      ?? throw new AlbumNotFoundException(command.AlbumId);

        AlbumRating? existing = await context.AlbumRatings
            .FirstOrDefaultAsync(r => r.UserId == command.UserId && r.AlbumId == albumId, cancellationToken);

        if (existing is not null)
            existing.Update(command.Rating);
        else
            await context.AlbumRatings.AddAsync(AlbumRating.Create(command.UserId, albumId, command.Rating), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        double? average = await context.AlbumRatings
            .Where(r => r.AlbumId == albumId)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);

        int count = await context.AlbumRatings
            .CountAsync(r => r.AlbumId == albumId, cancellationToken);

        album.UpdateRatingStats(average, count);
        await context.SaveChangesAsync(cancellationToken);

        return new RateAlbumResult(command.AlbumId, command.Rating, average, count);
    }
}
