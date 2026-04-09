using System.Security.Claims;
using Library.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Library.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        string currentUser = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
            ?? httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? "system";

        foreach (EntityEntry<IEntity> entity in context.ChangeTracker.Entries<IEntity>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedBy = currentUser;
                entity.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entity.State == EntityState.Added || entity.State == EntityState.Modified || entity.HasChangedOwnedEntities())
            {
                entity.Entity.LastModifiedBy = currentUser;
                entity.Entity.LastModifiedAt = DateTime.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entity) => entity.References.Any(a => a.TargetEntry != null && a.TargetEntry.Metadata.IsOwned() && a.TargetEntry.State is EntityState.Added or EntityState.Modified);
}