namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandBySlug;

public record GetBandBySlugQuery(string Slug, bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetBandBySlugResult>;
public record GetBandBySlugResult(BandDetailDto Band);
