namespace Library.Application.Services.Countries.Commands.DeleteCountry;

public class DeleteCountryCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteCountryCommand, DeleteCountryResult>
{
    public async ValueTask<DeleteCountryResult> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
    {
        Country country = await context.Countries.FindAsync([CountryId.Of(command.Id)], cancellationToken)
                          ?? throw new CountryNotFoundException(command.Id);

        context.Countries.Remove(country);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteCountryResult(true);
    }
}
