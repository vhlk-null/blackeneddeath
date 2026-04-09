namespace Library.Application.Services.Albums.Queries.GetPendingApprovalAlbums;

public record GetPendingApprovalAlbumsQuery() : BuildingBlocks.CQRS.IQuery<GetPendingApprovalAlbumsResult>;
public record GetPendingApprovalAlbumsResult(List<PendingApprovalDto> Albums);
