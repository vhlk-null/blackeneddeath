namespace Library.Application.Services.Labels.Queries.GetLabels;

public record GetLabelsQuery() : BuildingBlocks.CQRS.IQuery<GetLabelsResult>;

public record GetLabelsResult(IReadOnlyList<LabelDto> Labels);
