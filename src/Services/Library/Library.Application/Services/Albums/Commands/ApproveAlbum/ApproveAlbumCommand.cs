namespace Library.Application.Services.Albums.Commands.ApproveAlbum;

public record ApproveAlbumCommand(Guid AlbumId) : BuildingBlocks.CQRS.ICommand<ApproveAlbumResult>;
public record ApproveAlbumResult(bool IsSuccess);

public class ApproveAlbumCommandValidator : AbstractValidator<ApproveAlbumCommand>
{
    public ApproveAlbumCommandValidator()
    {
        RuleFor(x => x.AlbumId).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
    }
}
