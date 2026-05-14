using Hangfire;
using Library.Application.Data;
using Library.Application.Services.Albums.Jobs;

namespace Library.Application.Services.Albums.EventHandlers.Domain;

public class AlbumReleaseSchedulingHandler(IBackgroundJobClient jobClient, ILibraryDbContext context) : INotificationHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent notification, CancellationToken cancellationToken)
    {
        string? jobId = ScheduleJob(jobClient, notification.Album);
        if (jobId is not null)
        {
            notification.Album.SetHangfireJobId(jobId);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public static string? ScheduleJob(IBackgroundJobClient jobClient, Album album)
    {
        if (album.AlbumRelease.ReleaseYear == 0 || album.AlbumRelease.ReleaseMonth is null ||
            album.AlbumRelease.ReleaseDay is null) return null;

        var releaseDate = new DateTime(album.AlbumRelease.ReleaseYear, album.AlbumRelease.ReleaseMonth.Value, album.AlbumRelease.ReleaseDay.Value, 0, 0, 0, DateTimeKind.Utc);

        if (releaseDate <= DateTime.UtcNow) return null;

        if (album.HangfireJobId is not null)
            try { jobClient.Delete(album.HangfireJobId); } catch { /* job may not exist */ }

        Guid albumId = album.Id.Value;
        return jobClient.Schedule<IAlbumReleaseJob>(
            job => job.ExecuteAsync(albumId, album.Title, album.Slug, album.AlbumRelease.ReleaseYear, CancellationToken.None),
            releaseDate);
    }
}
