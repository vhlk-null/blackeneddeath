namespace UserContent.Application.UserContent.FavoriteBands.AddFavoriteBand;

public class AddFavoriteBandCommandHandler(IUserContentRepository repo)
    : ICommandHandler<AddBandToFavoriteCommand, AddBandToFavoriteResult>
{
    public async ValueTask<AddBandToFavoriteResult> Handle(AddBandToFavoriteCommand request, CancellationToken cancellationToken)
    {
        var favoriteBand = new FavoriteBand
        {
            UserId = request.UserId,
            BandId = request.BandId,
            AddedDate = DateTime.UtcNow
        };

        await repo.AddAsync(favoriteBand, cancellationToken);

        return new AddBandToFavoriteResult(request.UserId);
    }
}
