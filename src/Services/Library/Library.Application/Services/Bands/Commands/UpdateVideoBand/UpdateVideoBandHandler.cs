namespace Library.Application.Services.Bands.Commands.UpdateVideoBand;

public class UpdateVideoBandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateVideoBandCommand, UpdateVideoBandResult>
{
    public async ValueTask<UpdateVideoBandResult> Handle(UpdateVideoBandCommand command, CancellationToken cancellationToken)
    {
        UpdateVideoBandDto dto = command.VideoBand;

        VideoBand videoBand = await context.VideoBands.FindAsync([VideoBandId.Of(dto.Id)], cancellationToken)
                              ?? throw new VideoBandNotFoundException(dto.Id);

        if (dto.CountryId.HasValue)
        {
            bool countryExists = await context.Countries.AnyAsync(c => c.Id == CountryId.Of(dto.CountryId.Value), cancellationToken);
            if (!countryExists)
                throw new CountryNotFoundException(dto.CountryId.Value);
        }

        videoBand.Update(
            dto.Name,
            dto.Year,
            dto.CountryId.HasValue ? CountryId.Of(dto.CountryId.Value) : null,
            dto.VideoType,
            dto.YoutubeLink,
            dto.Info);
        videoBand.Approve();

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateVideoBandResult(true);
    }
}
