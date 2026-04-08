namespace Library.Application.Services.Labels.Queries.GetLabels;

public class GetLabelsQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetLabelsQuery, GetLabelsResult>
{
    public async ValueTask<GetLabelsResult> Handle(GetLabelsQuery query, CancellationToken cancellationToken)
    {
        List<Label> labels = await context.Labels
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        List<LabelDto> labelDtos = labels.Select(l => l.ToLabelDto()).ToList();

        return new GetLabelsResult(labelDtos);
    }
}
