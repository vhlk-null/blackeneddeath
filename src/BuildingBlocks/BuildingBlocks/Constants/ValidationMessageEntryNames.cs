namespace BuildingBlocks.Constants;

public static class ValidationMessageEntryNames
{
    public const string EmptyRequiredField = nameof(EmptyRequiredField);
    public const string MaxLengthIsExceeded = nameof(MaxLengthIsExceeded);
    public const string ValueNotWithinRange = nameof(ValueNotWithinRange);
    public const string NotUniqueCompoundNames = nameof(NotUniqueCompoundNames);
    public const string LowerGreaterThanUpper = nameof(LowerGreaterThanUpper);
    public const string InvalidFieldValue = nameof(InvalidFieldValue);
    public const string InvalidCharacters = nameof(InvalidCharacters);
    public const string ReleaseYearTooOld = nameof(ReleaseYearTooOld);
    public const string ReleaseYearInFuture = nameof(ReleaseYearInFuture);
    public const string ReleaseYearRequired = nameof(ReleaseYearRequired);
    public const string BandYearOutOfRange = nameof(BandYearOutOfRange);
    public const string DisbandedYearBeforeFormed = nameof(DisbandedYearBeforeFormed);
}