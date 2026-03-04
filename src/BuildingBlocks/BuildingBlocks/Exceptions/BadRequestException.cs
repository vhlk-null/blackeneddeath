namespace BuildingBlocks.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string messge, string detailes) : base(messge)
    {
        Details = detailes;
    }

    public string? Details { get; set; }
}