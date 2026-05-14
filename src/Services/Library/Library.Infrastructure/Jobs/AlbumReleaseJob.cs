using BuildingBlocks.Messaging.Events.Albums;
using Library.Application.Services.Albums.Jobs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Jobs;

public class AlbumReleaseJob(IPublishEndpoint publishEndpoint, ILogger<AlbumReleaseJob> logger) : IAlbumReleaseJob
{
    public async Task ExecuteAsync(Guid guild, string albumTitle, string ablumSlug, int releaseYear, CancellationToken cancellationToken)
    {
        logger.LogInformation("AlbumReleaseJob executing for AlbumId: {AlbumId}, Title: {Title}", guild, albumTitle);
        await publishEndpoint.Publish(new AlbumReleaseIntegrationEvent
        { AlbumId = guild, Title = albumTitle, Slug = ablumSlug, ReleaseYear = releaseYear }, cancellationToken);
        logger.LogInformation("AlbumReleaseJob published integration event for AlbumId: {AlbumId}", guild);
    }
}
