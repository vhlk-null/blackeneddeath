namespace Library.Domain.ValueObjects;

public record BandActivity
{
    public int? FormedYear { get; init; }
    public int? DisbandedYear { get; init; }

    private BandActivity(int? formedYear, int? disbandedYear)
    {
        FormedYear = formedYear;
        DisbandedYear = disbandedYear;
    }

    public static BandActivity Of(int? formedYear, int? disbandedYear)
    {
        if (formedYear is <= 0)
            throw new DomainException("Formed year must be a positive number.");

        if (disbandedYear is <= 0)
            throw new DomainException("Disbanded year must be a positive number.");

        if (formedYear.HasValue && disbandedYear.HasValue && disbandedYear <= formedYear)
            throw new DomainException("Disbanded year must be greater than formed year.");

        return new BandActivity(formedYear, disbandedYear);
    }
}
