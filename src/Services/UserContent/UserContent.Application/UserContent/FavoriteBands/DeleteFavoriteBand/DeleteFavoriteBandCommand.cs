namespace UserContent.Application.UserContent.FavoriteBands.DeleteFavoriteBand;

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
