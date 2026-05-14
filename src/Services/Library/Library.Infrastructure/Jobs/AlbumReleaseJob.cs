using BuildingBlocks.Messaging.Events.Albums;
using Library.Application.Services.Albums.Jobs;
using MassTransit;

namespace Library.Infrastructure.Jobs
{
    public class AlbumReleaseJob(IPublishEndpoint publishEndpoint) : IAlbumReleaseJob
    {
        public async Task ExecuteAsync(Guid guild, string albumTitle, string ablumSlug, int releaseYear, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(new AlbumReleaseIntegrationEvent()
            { AlbumId = guild, Title = albumTitle, Slug = ablumSlug, ReleaseYear = releaseYear }, cancellationToken);
        }
    }
}
