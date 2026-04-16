namespace Library.Application.Bands.Queries.GetBandsBy.GetBandBySlug;

public record GetBandBySlugQuery(string Slug) : BuildingBlocks.CQRS.IQuery<GetBandBySlugResult>;
public record GetBandBySlugResult(BandDto Band);
