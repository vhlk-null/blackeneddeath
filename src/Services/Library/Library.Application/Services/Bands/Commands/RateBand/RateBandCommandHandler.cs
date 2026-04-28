namespace Library.Application.Services.Bands.Commands.RateBand;

public class RateBandCommandHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<RateBandCommand, RateBandResult>
{
    public async ValueTask<RateBandResult> Handle(RateBandCommand command, CancellationToken cancellationToken)
    {
        BandId bandId = BandId.Of(command.BandId);

        Band band = await context.Bands
                        .FirstOrDefaultAsync(b => b.Id == bandId, cancellationToken)
                    ?? throw new BandNotFoundException(command.BandId);

        BandRating? existing = await context.BandRatings
            .FirstOrDefaultAsync(r => r.UserId == command.UserId && r.BandId == bandId, cancellationToken);

        if (existing is not null)
            existing.Update(command.Rating);
        else
            await context.BandRatings.AddAsync(BandRating.Create(command.UserId, bandId, command.Rating), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        double? average = await context.BandRatings
            .Where(r => r.BandId == bandId)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);

        int count = await context.BandRatings
            .CountAsync(r => r.BandId == bandId, cancellationToken);

        band.UpdateRatingStats(average, count);
        await context.SaveChangesAsync(cancellationToken);

        return new RateBandResult(command.BandId, command.Rating, average, count);
    }
}
