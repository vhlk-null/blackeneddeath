namespace Library.Application.Exceptions;

public class GenreCardNotFoundException(Guid id) : NotFoundException("GenreCard", id);
