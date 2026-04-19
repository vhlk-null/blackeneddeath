namespace UserContent.Application.Exceptions;

public class CollectionNotFoundException : NotFoundException
{
    public CollectionNotFoundException(Guid id) : base("Collection", id)
    {
    }
}
