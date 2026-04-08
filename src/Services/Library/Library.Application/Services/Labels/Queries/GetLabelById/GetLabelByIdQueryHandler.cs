namespace Library.Application.Services.Labels.Queries.GetLabelById;

public class GetLabelByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetLabelByIdQuery, GetLabelByIdResult>
{
    public async ValueTask<GetLabelByIdResult> Handle(GetLabelByIdQuery query, CancellationToken cancellationToken)
    {
        LabelId labelId = LabelId.Of(query.Id);

        Label label = await context.Labels
                          .AsNoTracking()
                          .FirstOrDefaultAsync(l => l.Id == labelId, cancellationToken)
                      ?? throw new LabelNotFoundException(query.Id);

        return new GetLabelByIdResult(label.ToLabelDto());
    }
}
