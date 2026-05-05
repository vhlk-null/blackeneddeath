namespace Notifications.Domain.Models;

public class Subscription
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = default!;
    public string ResourceType { get; private set; } = default!;
    public string ResourceId { get; private set; } = default!;
    public string ResourceName { get; private set; } = default!;
    public string ResourceSlug { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    private Subscription() { }

    public static Subscription Create(string userId, string resourceType, string resourceId, string resourceName, string resourceSlug)
    {
        return new Subscription
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ResourceType = resourceType,
            ResourceId = resourceId,
            ResourceName = resourceName,
            ResourceSlug = resourceSlug,
            CreatedAt = DateTime.UtcNow
        };
    }
}
