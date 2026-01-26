namespace Archive.API.Albums.UpdateAlbum
{
    public record UpdateAlbumCommand(Guid Id,
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds) : ICommand<UpdateAlbumResult>;

    public record UpdateAlbumResult(bool IsSuccess);

    public class UpdateAlbumCommandValidator : AbstractValidator<UpdateAlbumCommand>
    {
        public UpdateAlbumCommandValidator()
        {
            RuleFor(x => x.Title)
             .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
             .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.Label)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
             .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
                .WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.ReleaseDate)
                .GreaterThan(0).WithMessage(ValidationMessages.ReleaseYearRequired)
                .GreaterThan(1900).WithMessage(ValidationMessages.ReleaseYearTooOld)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage(ValidationMessages.ReleaseYearInFuture);
        }
    }

    internal class UpdateAlbumCommandHandler(IRepository<ArchiveContext> repo) : ICommandHandler<UpdateAlbumCommand, UpdateAlbumResult>
    {
        public async Task<UpdateAlbumResult> Handle(UpdateAlbumCommand command, CancellationToken cancellationToken)
        {
            var album = await repo.GetByAsync<Album>(a => a.Id == command.Id) ?? throw new AlbumNotFoundException(command.Id);

            album.Title = command.Title;
            album.ReleaseDate = command.ReleaseDate;
            album.Type = command.Type;
            album.Format = command.Format;
            album.Label = command.Label;

            repo.Update(album);
            await repo.SaveChangesAsync();

            return new UpdateAlbumResult(true);
        }
    }
}
