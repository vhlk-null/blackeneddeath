namespace Library.Domain.Events.Album;

public record AlbumRemovedEvent(Models.Album Album) : IAlbumDomainEvent;
