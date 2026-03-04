using System.Globalization;
using System.Reflection;
using System.Resources;
using BuildingBlocks.Constants;

namespace Library.Application.Resources.ResourceManagement;

public static class ValidationMessages
{
    private static ResourceManager resourceManager;

    static ValidationMessages()
    {
        resourceManager = new ResourceManager(
            $"{GeneralResourceConstants.ResourceFolderPath}.{GeneralResourceConstants.ValidationMessagesResourceFileName}",
            Assembly.GetExecutingAssembly());
    }

    public static string EmptyRequiredField =>
        resourceManager.GetString(ValidationMessageEntryNames.EmptyRequiredField, CultureInfo.CurrentCulture)!;

    public static string MaxLengthIsExceeded =>
        resourceManager.GetString(ValidationMessageEntryNames.MaxLengthIsExceeded, CultureInfo.CurrentCulture)!;

    public static string ValueNotWithinRange =>
        resourceManager.GetString(ValidationMessageEntryNames.ValueNotWithinRange, CultureInfo.CurrentCulture)!;

    public static string InvalidFieldValue =>
        resourceManager.GetString(ValidationMessageEntryNames.InvalidFieldValue, CultureInfo.CurrentCulture)!;

    public static string InvalidCharacters =>
        resourceManager.GetString(ValidationMessageEntryNames.InvalidCharacters, CultureInfo.CurrentCulture)!;

    public static string ReleaseYearTooOld =>
        resourceManager.GetString(ValidationMessageEntryNames.ReleaseYearTooOld, CultureInfo.CurrentCulture)!;

    public static string ReleaseYearInFuture =>
        resourceManager.GetString(ValidationMessageEntryNames.ReleaseYearInFuture, CultureInfo.CurrentCulture)!;

    public static string ReleaseYearRequired =>
        resourceManager.GetString(ValidationMessageEntryNames.ReleaseYearRequired, CultureInfo.CurrentCulture)!;
}
