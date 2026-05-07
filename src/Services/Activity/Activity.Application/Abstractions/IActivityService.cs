namespace Activity.Application.Abstractions;

public interface IActivityService
{
    Task RecordAsync(Guid userId, string type, Dictionary<string, string> payload, CancellationToken ct = default);
    Task<IReadOnlyList<UserActivityDto>> GetUserActivityAsync(Guid userId, CancellationToken ct = default);
}
