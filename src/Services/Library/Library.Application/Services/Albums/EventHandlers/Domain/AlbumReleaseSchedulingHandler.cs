using Hangfire;
using Library.Application.Services.Albums.Jobs;

namespace Library.Application.Services.Albums.EventHandlers.Domain;

public class AlbumReleaseSchedulingHandler(IBackgroundJobClient jobClient) : INotificationHandler<AlbumCreatedEvent>
{
    public ValueTask Handle(AlbumCreatedEvent notification, CancellationToken cancellationToken)
    {
        ScheduleJob(jobClient, notification.Album);
        return ValueTask.CompletedTask;
    }

    public static void ScheduleJob(IBackgroundJobClient jobClient, Album album)
    {
        if (album.AlbumRelease.ReleaseYear == 0 || album.AlbumRelease.ReleaseMonth is null ||
            album.AlbumRelease.ReleaseDay is null) return;

        var releaseDate = new DateTime(album.AlbumRelease.ReleaseYear, album.AlbumRelease.ReleaseMonth.Value, album.AlbumRelease.ReleaseDay.Value, 0, 0, 0, DateTimeKind.Utc);

        if (releaseDate <= DateTime.UtcNow) return;

        Guid albumId = album.Id.Value;
        string jobId = $"album-release-{albumId}";

        try { jobClient.Delete(jobId); } catch { /* job may not exist */ }
        jobClient.Schedule<IAlbumReleaseJob>(
            jobId,
            job => job.ExecuteAsync(albumId, album.Title, album.Slug, album.AlbumRelease.ReleaseYear, CancellationToken.None),
            releaseDate);
    }
}
