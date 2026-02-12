namespace UserContent.API.UserContent.FavoriteBands.DeleteUserFavoriteBands
{
    public record DeleteFavoriteBandCommand(Guid UserId, Guid BandId) : ICommand<DeleteFavoriteBandResult>;
    public record DeleteFavoriteBandResult(bool IsSuccess);

    public class DeleteFavoriteBandCommandValidator : AbstractValidator<DeleteFavoriteBandCommand>
    {
        public DeleteFavoriteBandCommandValidator()
        {
            RuleFor(x => x.BandId).NotEmpty().WithMessage("Band ID is required.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }

    public class DeleteFavoriteBandCommandHandler(IRepository<UserContentContext> repo) : ICommandHandler<DeleteFavoriteBandCommand, DeleteFavoriteBandResult>
    {
        public async ValueTask<DeleteFavoriteBandResult> Handle(DeleteFavoriteBandCommand request, CancellationToken cancellationToken)
        {
            var favoriteBand = await repo.GetByAsync<FavoriteBand>(
                fb => fb.BandId == request.BandId && fb.UserId == request.UserId, cancellationToken: cancellationToken)
                ?? throw new FavoriteBandNotFoundException(request.BandId);

            repo.Delete(favoriteBand);

            return new DeleteFavoriteBandResult(true);
        }
    }
}
