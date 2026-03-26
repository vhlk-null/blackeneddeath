namespace Library.Application.Services.Labels.Queries.GetLabelById;

public record GetLabelByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetLabelByIdResult>;

public record GetLabelByIdResult(LabelDto Label);
