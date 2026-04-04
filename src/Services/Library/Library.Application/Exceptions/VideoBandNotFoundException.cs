namespace Library.Application.Exceptions;

public class VideoBandNotFoundException(Guid id) : NotFoundException("VideoBand", id);
