namespace Library.Application.Services.Countries.Commands.UpdateCountry;

public class UpdateCountryCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateCountryCommand, UpdateCountryResult>
{
    public async ValueTask<UpdateCountryResult> Handle(UpdateCountryCommand command, CancellationToken cancellationToken)
    {
        var country = await context.Countries.FindAsync([CountryId.Of(command.Id)], cancellationToken)
            ?? throw new CountryNotFoundException(command.Id);

        country.Update(command.Name, command.Code);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateCountryResult(true);
    }
}
