namespace Notifications.Infrastructure.Data;

public class NotificationsContext(DbContextOptions<NotificationsContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationsContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
