namespace Library.Application.Exceptions;

public class GenreNotFoundException(Guid id) : NotFoundException("Genre", id);
