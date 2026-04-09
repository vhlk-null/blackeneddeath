namespace Library.Application.Dtos;

public record PendingApprovalDto(Guid Id, string Name, string? Slug, string? CreatedBy);
