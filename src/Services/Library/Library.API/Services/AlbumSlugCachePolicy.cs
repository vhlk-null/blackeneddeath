using Library.API.Constants;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.API.Services;

public sealed class AlbumSlugCachePolicy : IOutputCachePolicy
{
    public static readonly AlbumSlugCachePolicy Instance = new();

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        string? slug = context.HttpContext.GetRouteValue("slug")?.ToString();
        if (slug is not null)
            context.Tags.Add(OutputCacheTags.Album(slug));

        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.AllowLocking = true;
        context.ResponseExpirationTimeSpan = TimeSpan.FromDays(7);

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        if (context.HttpContext.Response.StatusCode != StatusCodes.Status200OK)
            context.AllowCacheStorage = false;
        return ValueTask.CompletedTask;
    }
}
