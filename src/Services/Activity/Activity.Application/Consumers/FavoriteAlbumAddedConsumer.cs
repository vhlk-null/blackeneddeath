using BuildingBlocks.Messaging.Events.Albums;

namespace Activity.Application.Consumers;

public class FavoriteAlbumAddedConsumer(IActivityService activityService) : IConsumer<AlbumCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AlbumCreatedIntegrationEvent> context)
    {
        // placeholder: wire up your own UserContent integration events here
        await Task.CompletedTask;
    }
}
