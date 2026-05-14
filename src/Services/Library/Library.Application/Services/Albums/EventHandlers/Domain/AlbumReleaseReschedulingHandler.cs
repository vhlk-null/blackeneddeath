using Hangfire;
using Library.Application.Services.Albums.Jobs;

namespace Library.Application.Services.Albums.EventHandlers.Domain;

public class AlbumReleaseReschedulingHandler(IBackgroundJobClient jobClient) : INotificationHandler<AlbumUpdatedEvent>
{
    public ValueTask Handle(AlbumUpdatedEvent notification, CancellationToken cancellationToken)
    {
        AlbumReleaseSchedulingHandler.ScheduleJob(jobClient, notification.Album);
        return ValueTask.CompletedTask;
    }
}
