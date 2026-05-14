using Hangfire;
using Library.Application.Data;
using Library.Application.Services.Albums.Jobs;

namespace Library.Application.Services.Albums.EventHandlers.Domain;

public class AlbumReleaseReschedulingHandler(IBackgroundJobClient jobClient, ILibraryDbContext context) : INotificationHandler<AlbumUpdatedEvent>
{
    public async ValueTask Handle(AlbumUpdatedEvent notification, CancellationToken cancellationToken)
    {
        string? jobId = AlbumReleaseSchedulingHandler.ScheduleJob(jobClient, notification.Album);
        if (jobId is not null)
        {
            notification.Album.SetHangfireJobId(jobId);
            await context.SaveChangesAsync(cancellationToken);
        }
        else if (notification.Album.HangfireJobId is not null)
        {
            notification.Album.SetHangfireJobId(null);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
