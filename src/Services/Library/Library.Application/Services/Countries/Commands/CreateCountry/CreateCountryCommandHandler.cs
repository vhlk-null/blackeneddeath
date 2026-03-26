namespace Library.Application.Services.Countries.Commands.CreateCountry;

public class CreateCountryCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateCountryCommand, CreateCountryResult>
{
    public async ValueTask<CreateCountryResult> Handle(CreateCountryCommand command, CancellationToken cancellationToken)
    {
        var country = Country.Create(CountryId.Of(Guid.NewGuid()), command.Name, command.Code);

        context.Countries.Add(country);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateCountryResult(country.Id.Value);
    }
}
