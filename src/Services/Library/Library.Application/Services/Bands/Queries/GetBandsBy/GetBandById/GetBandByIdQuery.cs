namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandById;

public record GetBandByIdQuery(Guid Id, bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetBandByIdResult>;
public record GetBandByIdResult(BandDetailDto Band);
