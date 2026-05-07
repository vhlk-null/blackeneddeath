using Microsoft.AspNetCore.Builder;

namespace Activity.Infrastructure.Data.Extensions;

public static class MongoIndexInitializer
{
    public static async Task EnsureMongoIndexesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var col = db.GetCollection<UserActivity>("user_activities");

        var index = Builders<UserActivity>.IndexKeys
            .Ascending(a => a.UserId)
            .Descending(a => a.OccurredAt);

        await col.Indexes.CreateOneAsync(new CreateIndexModel<UserActivity>(index));
    }
}
