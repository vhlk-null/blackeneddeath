namespace Library.Application.Exceptions;

public class CountryNotFoundException(Guid id) : NotFoundException("Country", id);
