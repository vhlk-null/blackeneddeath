namespace Library.Application.Exceptions;

public class AlbumNotFoundException(Guid id) : NotFoundException("Album", id);
