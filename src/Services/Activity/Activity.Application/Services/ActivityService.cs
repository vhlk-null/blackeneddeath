using Activity.Application.Abstractions;
using Activity.Application.Dtos;
using Activity.Domain.Models;
using BuildingBlocks.MongoDB.Repositories;

namespace Activity.Application.Services;

public class ActivityService(IMongoRepository<UserActivity> repository) : IActivityService
{
    public async Task RecordAsync(Guid userId, string type, Dictionary<string, string> payload, CancellationToken ct = default)
    {
        var activity = UserActivity.Create(userId, type, payload);
        await repository.AddAsync(activity, ct);
    }

    public async Task<IReadOnlyList<UserActivityDto>> GetUserActivityAsync(Guid userId, CancellationToken ct = default)
    {
        var activities = await repository.FilterAsync(a => a.UserId == userId, ct);
        return activities
            .OrderByDescending(a => a.OccurredAt)
            .Select(a => new UserActivityDto(a.Id, a.UserId, a.Type, a.OccurredAt, a.Payload))
            .ToList();
    }
}
