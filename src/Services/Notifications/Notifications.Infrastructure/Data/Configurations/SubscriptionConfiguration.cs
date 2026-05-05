namespace Notifications.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(s => s.ResourceType).HasColumnName("resource_type").IsRequired();
        builder.Property(s => s.ResourceId).HasColumnName("resource_id").IsRequired();
        builder.Property(s => s.ResourceName).HasColumnName("resource_name").IsRequired();
        builder.Property(s => s.ResourceSlug).HasColumnName("resource_slug").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(s => new { s.UserId, s.ResourceType, s.ResourceId })
            .IsUnique()
            .HasDatabaseName("ix_subscriptions_user_resource");

        builder.ToTable("subscriptions");
    }
}
