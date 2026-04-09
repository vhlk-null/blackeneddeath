namespace Library.Application.Services.Bands.Queries.GetPendingApprovalVideoBands;

public record GetPendingApprovalVideoBandsQuery() : BuildingBlocks.CQRS.IQuery<GetPendingApprovalVideoBandsResult>;
public record GetPendingApprovalVideoBandsResult(List<PendingApprovalDto> VideoBands);
