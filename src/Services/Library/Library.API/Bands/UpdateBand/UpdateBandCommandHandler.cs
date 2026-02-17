using Library.API.Data;
using Library.API.Exceptions;
using Library.API.Models;
using Library.API.Resources.ResourceManagement;

namespace Library.API.Bands.UpdateBand
{
    public record UpdateBandCommand(
        Guid Id,
        string Name,
        string? Bio,
        Guid? CountryId,
        int? FormedYear,
        int? DisbandedYear,
        BandStatus Status,
        List<Guid> GenreIds) : ICommand<UpdateBandResult>;

    public record UpdateBandResult(bool IsSuccess);

    public class UpdateBandCommandValidator : AbstractValidator<UpdateBandCommand>
    {
        public UpdateBandCommandValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
             .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.FormedYear)
                .GreaterThan(0).When(x => x.FormedYear.HasValue)
                .GreaterThan(1900).When(x => x.FormedYear.HasValue)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.FormedYear.HasValue);

            RuleFor(x => x.DisbandedYear)
                .GreaterThan(0).When(x => x.DisbandedYear.HasValue)
                .GreaterThan(1900).When(x => x.DisbandedYear.HasValue)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).When(x => x.DisbandedYear.HasValue)
                .GreaterThanOrEqualTo(x => x.FormedYear).When(x => x.FormedYear.HasValue && x.DisbandedYear.HasValue)
                .WithMessage("Disbanded year must be after formed year");
        }
    }

    internal class UpdateBandCommandHandler(IRepository<LibraryContext> repo) : ICommandHandler<UpdateBandCommand, UpdateBandResult>
    {
        public async ValueTask<UpdateBandResult> Handle(UpdateBandCommand command, CancellationToken cancellationToken)
        {
            var band = await repo.GetByAsync<Band>(b => b.Id == command.Id, cancellationToken: cancellationToken)
                ?? throw new BandNotFoundException(command.Id);

            band.Name = command.Name;
            band.Bio = command.Bio;
            band.CountryId = command.CountryId;
            band.FormedYear = command.FormedYear;
            band.DisbandedYear = command.DisbandedYear;
            band.Status = command.Status;

            repo.Update(band);

            return new UpdateBandResult(true);
        }
    }
}
