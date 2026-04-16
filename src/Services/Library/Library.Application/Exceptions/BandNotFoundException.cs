namespace Library.Application.Exceptions;

public class BandNotFoundException(Guid id) : NotFoundException("Band", id);
public class BandBySlugNotFoundException(string slug) : NotFoundException("Band", slug);
