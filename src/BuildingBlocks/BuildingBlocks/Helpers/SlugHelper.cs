using System.Text.RegularExpressions;

namespace BuildingBlocks.Helpers;

public static class SlugHelper
{
    public static string Generate(string input) =>
        Regex.Replace(input.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
