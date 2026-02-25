using Library.API.Data;
using Library.API.Exceptions;
using Library.API.Resources.ResourceManagement;
using Library.Domain.Models;
using Library.Infrastructure.Data;

namespace Library.API.Bands.DeleteBand;

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
    IRepository<LibraryContext> repo,
    ILogger<DeleteBandCommandHandler> logger)
    : ICommandHandler<DeleteBandCommand, DeleteBandResult>
{
    public async ValueTask<DeleteBandResult> Handle(
        DeleteBandCommand command,
        CancellationToken cancellationToken)
    {
        var band = await repo.GetByAsync<Band>(
            b => b.Id.Value == command.Id,
            cancellationToken: cancellationToken);

        if (band == null) throw new BandNotFoundException(command.Id);

        repo.Delete(band);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Band {BandId} deleted successfully", command.Id);

        return new DeleteBandResult(true);
    }
}