namespace Library.Application.Services.Tags.Queries.GetTags;

public record GetTagsQuery() : BuildingBlocks.CQRS.IQuery<GetTagsResult>;

public record GetTagsResult(IReadOnlyList<TagDto> Tags);
