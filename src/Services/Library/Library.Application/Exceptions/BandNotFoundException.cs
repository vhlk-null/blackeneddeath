namespace Library.Application.Exceptions;

public class BandNotFoundException(Guid id) : NotFoundException("Band", id);
