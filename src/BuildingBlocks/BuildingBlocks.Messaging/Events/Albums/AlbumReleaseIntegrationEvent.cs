using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Messaging.Events.Albums
{
    public class AlbumReleaseIntegrationEvent : IntegrationEvent
    {
        public Guid AlbumId { get; init; }
        public string Title { get; init; } = null!;
        public string? Slug { get; init; }
        public int ReleaseYear { get; init; }
    }
}
