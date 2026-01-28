using Archive.API.Resources.ResourceManagement;
using BuildingBlocks.Models;

namespace Archive.API.Bands.DeleteBand
{
    public record DeleteBandCommand(Guid Id) : ICommand<DeleteBandResult>;
    public record DeleteBandResult(bool IsSuccess);

    public class DeleteBandCommandValidator : AbstractValidator<DeleteBandCommand>
    {
        public DeleteBandCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField);
        }
    }

    internal class DeleteBandCommandHandler(
        IRepository<ArchiveContext> repo,
        ILogger<DeleteBandCommandHandler> logger)
        : ICommandHandler<DeleteBandCommand, DeleteBandResult>
    {
        public async Task<DeleteBandResult> Handle(
            DeleteBandCommand command,
            CancellationToken cancellationToken)
        {
            var band = await repo.GetByAsync<Band>(
                b => b.Id == command.Id,
                cancellationToken: cancellationToken);

            if (band == null) throw new BandNotFoundException(command.Id);

            repo.Delete(band);
            await repo.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Band {BandId} deleted successfully", command.Id);

            return new DeleteBandResult(true);
        }
    }
}
