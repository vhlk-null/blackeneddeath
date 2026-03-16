namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandById;

public record GetBandByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetBandByIdResult>;
public record GetBandByIdResult(BandDto Band);
