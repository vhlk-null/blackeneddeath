namespace UserContent.Application.UserContent.FavoriteBands.DeleteFavoriteBand;

public class DeleteFavoriteBandCommandHandler(IUserContentRepository repo)
    : ICommandHandler<DeleteFavoriteBandCommand, DeleteFavoriteBandResult>
{
    public async ValueTask<DeleteFavoriteBandResult> Handle(DeleteFavoriteBandCommand request, CancellationToken cancellationToken)
    {
        var favoriteBand = await repo.GetFavoriteBandAsync(request.UserId, request.BandId, cancellationToken)
            ?? throw new FavoriteBandNotFoundException(request.BandId);

        repo.Remove(favoriteBand);

        return new DeleteFavoriteBandResult(true);
    }
}
