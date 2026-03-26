namespace Library.Application.Services.Tags.Queries.GetTagById;

public record GetTagByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetTagByIdResult>;

public record GetTagByIdResult(TagDto Tag);
