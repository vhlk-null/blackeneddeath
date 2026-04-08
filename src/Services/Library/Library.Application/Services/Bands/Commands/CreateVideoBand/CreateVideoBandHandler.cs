namespace Library.Application.Services.Bands.Commands.CreateVideoBand;

public class CreateVideoBandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateVideoBandCommand, CreateVideoBandResult>
{
    public async ValueTask<CreateVideoBandResult> Handle(CreateVideoBandCommand command, CancellationToken cancellationToken)
    {
        CreateVideoBandDto dto = command.VideoBand;

        bool bandExists = await context.Bands.AnyAsync(b => b.Id == BandId.Of(dto.BandId), cancellationToken);
        if (!bandExists)
            throw new BandNotFoundException(dto.BandId);

        if (dto.CountryId.HasValue)
        {
            bool countryExists = await context.Countries.AnyAsync(c => c.Id == CountryId.Of(dto.CountryId.Value), cancellationToken);
            if (!countryExists)
                throw new CountryNotFoundException(dto.CountryId.Value);
        }

        VideoBand videoBand = VideoBand.Create(
            BandId.Of(dto.BandId),
            dto.Name,
            dto.Year,
            dto.CountryId.HasValue ? CountryId.Of(dto.CountryId.Value) : null,
            dto.VideoType,
            dto.YoutubeLink,
            dto.Info);

        context.VideoBands.Add(videoBand);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateVideoBandResult(videoBand.Id.Value);
    }
}
