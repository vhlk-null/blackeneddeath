using Library.API.Data;
using Library.API.Resources.ResourceManagement;
using Library.Domain.Models;

namespace Library.API.Bands.CreateBand
{
    public record CreateBandCommand(
        string Name,
        string? Bio,
        Guid? CountryId,
        int? FormedYear,
        int? DisbandedYear,
        BandStatus Status,
        List<Guid> GenreIds
    ) : ICommand<CreateBandResult>;

    public record CreateBandResult(Guid Id);

    public class CreateBandCommandValidator : AbstractValidator<CreateBandCommand>
    {
        public CreateBandCommandValidator()
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

    internal class CreateBandCommandHandler(IRepository<LibraryContext> repo)
        : ICommandHandler<CreateBandCommand, CreateBandResult>
    {
        public async ValueTask<CreateBandResult> Handle(CreateBandCommand command, CancellationToken cancellationToken)
        {
            var band = new Band
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Bio = command.Bio,
                CountryId = command.CountryId,
                FormedYear = command.FormedYear,
                DisbandedYear = command.DisbandedYear,
                Status = command.Status
            };

            await repo.AddAsync(band, cancellationToken);

            return new CreateBandResult(band.Id);
        }
    }
}
