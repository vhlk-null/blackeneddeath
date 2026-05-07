namespace Activity.Infrastructure.Repositories;

public class ActivityRepository(IMongoDatabase database)
    : BaseMongoRepository<UserActivity>(database, "user_activities");
