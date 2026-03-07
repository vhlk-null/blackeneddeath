namespace Library.Domain.Events.Album;

public record AlbumCreatedEvent(Models.Album Album) : IAlbumDomainEvent;
