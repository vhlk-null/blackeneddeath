namespace Library.Application.Services.Albums.Queries.GetPendingApprovalAlbums;

public record GetPendingApprovalAlbumsQuery() : BuildingBlocks.CQRS.IQuery<GetPendingApprovalAlbumsResult>;
public record GetPendingApprovalAlbumsResult(List<PendingApprovalBandGroup> Groups);
public record PendingApprovalBandGroup(Guid BandId, string BandName, string? BandSlug, List<PendingApprovalDto> Albums);
