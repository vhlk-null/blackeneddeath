namespace Library.Domain.Events.Album;

public interface IAlbumDomainEvent : IDomainEvent
{
    Models.Album Album { get; }
}
