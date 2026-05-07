namespace Activity.Domain.Models;

public class UserActivity
{
    public string Id { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public string Type { get; private set; } = null!;
    public DateTime OccurredAt { get; private set; }
    public Dictionary<string, string> Payload { get; private set; } = [];

    private UserActivity() { }

    public static UserActivity Create(Guid userId, string type, Dictionary<string, string> payload)
    {
        return new UserActivity
        {
            Id = Guid.NewGuid().ToString("N"),
            UserId = userId,
            Type = type,
            OccurredAt = DateTime.UtcNow,
            Payload = payload
        };
    }
}
