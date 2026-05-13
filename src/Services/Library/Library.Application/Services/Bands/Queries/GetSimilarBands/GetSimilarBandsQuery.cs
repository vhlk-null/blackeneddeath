namespace Library.Application.Services.Bands.Queries.GetSimilarBands;

public record GetSimilarBandsQuery(string Slug, bool ApprovedOnly = true, int PageNumber = 1, int PageSize = 4)
    : BuildingBlocks.CQRS.IQuery<GetSimilarBandsResult>;

public record GetSimilarBandsResult(PaginatedResult<BandCardDto> SimilarBands);
