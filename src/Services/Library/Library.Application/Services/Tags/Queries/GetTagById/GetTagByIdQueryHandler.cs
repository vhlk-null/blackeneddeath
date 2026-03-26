namespace Library.Application.Services.Tags.Queries.GetTagById;

public class GetTagByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetTagByIdQuery, GetTagByIdResult>
{
    public async ValueTask<GetTagByIdResult> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
    {
        var tagId = TagId.Of(query.Id);

        var tag = await context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tagId, cancellationToken)
            ?? throw new TagNotFoundException(query.Id);

        return new GetTagByIdResult(tag.ToTagDto());
    }
}
