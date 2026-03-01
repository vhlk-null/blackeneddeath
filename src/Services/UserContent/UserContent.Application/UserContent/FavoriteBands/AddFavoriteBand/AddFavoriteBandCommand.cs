namespace UserContent.Application.UserContent.FavoriteBands.AddFavoriteBand;

public record AddBandToFavoriteCommand(Guid BandId, Guid UserId) : ICommand<AddBandToFavoriteResult>;
public record AddBandToFavoriteResult(Guid UserId);

public class AddBandToFavoriteCommandValidator : AbstractValidator<AddBandToFavoriteCommand>
{
    public AddBandToFavoriteCommandValidator()
    {
        RuleFor(x => x.BandId).NotEmpty().WithMessage("Band ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}
