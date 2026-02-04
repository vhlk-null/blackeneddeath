namespace UserContent.API.UserContent.FavoriteBands.AddFavoriteBand
{
    public record AddBandToFavoriteCommand(Guid bandId, Guid userId) : ICommand<AddBandToFavoriteResult>;
    public record AddBandToFavoriteResult(Guid userId);

    public class AddBandToFavoriteCommandValidator : AbstractValidator<AddBandToFavoriteCommand>
    {
        public AddBandToFavoriteCommandValidator()
        {
            RuleFor(x => x.bandId).NotEmpty().WithMessage("Band ID is required.");
            RuleFor(x => x.userId).NotEmpty().WithMessage("User ID is required.");
        }
    }

    public class AddFavoriteBandCommandHandler(IRepository<UserContentContext> repo) : ICommandHandler<AddBandToFavoriteCommand, AddBandToFavoriteResult>
    {
        public async ValueTask<AddBandToFavoriteResult> Handle(AddBandToFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favoriteBand = new FavoriteBand()
            {
                UserId = request.userId,
                BandId = request.bandId,
                BandName = "test"
            };

            await repo.AddAsync(favoriteBand, cancellationToken);

            return new AddBandToFavoriteResult(request.userId);
        }
    }
}
