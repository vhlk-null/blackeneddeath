namespace Library.Application.Services.Bands.Queries.GetPendingApprovalBands;

public record GetPendingApprovalBandsQuery() : BuildingBlocks.CQRS.IQuery<GetPendingApprovalBandsResult>;
public record GetPendingApprovalBandsResult(List<PendingApprovalDto> Bands);
