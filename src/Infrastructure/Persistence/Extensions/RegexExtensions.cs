using System.Text.RegularExpressions;

namespace Ecommerce.Infrastructure.Persistence.Extensions;

public static class RegexExtensions
{
    private static readonly Regex WhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

    public static string ReplaceWhiteSpace(this string value, string replacement)
    {
        return WhiteSpaceRegex.Replace(value, replacement);
    }
}