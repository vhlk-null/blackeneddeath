namespace Library.Domain.Events.Album;

public record AlbumUpdatedEvent(Models.Album Album) : IAlbumDomainEvent;
