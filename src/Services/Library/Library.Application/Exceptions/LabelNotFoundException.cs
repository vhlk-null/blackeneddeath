namespace Library.Application.Exceptions;

public class LabelNotFoundException(Guid id) : NotFoundException("Label", id);
