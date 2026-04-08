namespace Library.Application.Services.Tags.Queries.GetTags;

public class GetTagsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetTagsQuery, GetTagsResult>
{
    public async ValueTask<GetTagsResult> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        List<Tag> tags = await context.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);

        return new GetTagsResult(tags.Select(t => t.ToTagDto()).ToList());
    }
}
