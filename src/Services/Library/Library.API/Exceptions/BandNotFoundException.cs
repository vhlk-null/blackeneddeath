namespace Library.API.Exceptions;

public class BandNotFoundException : Exception
{
    public BandNotFoundException(Guid id) : base($"Band with ID {id} was not found.")
    {
    }
}