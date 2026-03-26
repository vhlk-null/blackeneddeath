namespace Library.Application.Services.Labels.Queries.GetLabels;

public class GetLabelsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetLabelsQuery, GetLabelsResult>
{
    public async ValueTask<GetLabelsResult> Handle(GetLabelsQuery query, CancellationToken cancellationToken)
    {
        var labels = await context.Labels
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        var labelDtos = labels.Select(l => l.ToLabelDto()).ToList();

        return new GetLabelsResult(labelDtos);
    }
}
