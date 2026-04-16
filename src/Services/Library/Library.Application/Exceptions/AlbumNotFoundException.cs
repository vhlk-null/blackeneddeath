namespace Library.Application.Exceptions;

public class AlbumNotFoundException(Guid id) : NotFoundException("Album", id);
public class AlbumBySlugNotFoundException(string slug) : NotFoundException("Album", slug);
