using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Ecommerce.Infrastructure.Data.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        object[] attrs = enumValue.GetType().GetField(enumValue.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attrs.Length > 0)
        {
            return ((DescriptionAttribute)attrs[0]).Description;
        }

        string result = enumValue.ToString();
        result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");
        result = Regex.Replace(result, "([A-Z])([A-Z][a-z])", "$1 $2");
        result = Regex.Replace(result, "([0-9])([A-Za-z])", "$1 $2");
        result = Regex.Replace(result, "(?<!^)(?<! )([A-Z][a-z])", " $1");
        return result;
    }

    public static List<string> GetDescriptionList(this Enum enumValue)
    {
        string result = enumValue.GetDescription();
        return result.Split(',').ToList();
    }
}