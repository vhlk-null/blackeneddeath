namespace Library.Domain.ValueObjects;

public record BandActivity
{
    public int? FormedYear { get; init; }
    public int? DisbandedYear { get; init; }
}
