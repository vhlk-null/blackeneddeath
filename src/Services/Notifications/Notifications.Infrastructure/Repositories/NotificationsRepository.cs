namespace Notifications.Infrastructure.Repositories;

public class NotificationsRepository : BaseGenericRepository<NotificationsContext>
{
    public NotificationsRepository(NotificationsContext context)
    {
        Context = context;
    }
}
