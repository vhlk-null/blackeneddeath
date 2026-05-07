namespace Activity.Application.Dtos;

public record UserActivityDto(
    string Id,
    Guid UserId,
    string Type,
    DateTime OccurredAt,
    Dictionary<string, string> Payload);
