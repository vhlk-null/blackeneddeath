namespace Library.Application.Exceptions;

public class TagNotFoundException(Guid id) : NotFoundException("Tag", id);
