using System.ComponentModel.DataAnnotations;
using Spectre.Console;

namespace PhoneBook.Util;

public static class Helpers
{
    public static string GetEnumDisplayValue<T>(T enumValue) where T : Enum
    {
        var displayAttribute = enumValue
            .GetType()
            .GetField(enumValue.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? enumValue.ToString();
    }

    public static Rule GetStandardRule(string title) =>
        new Rule
        {
            Title = $"[darkgoldenrod]{title}[/]",
            Justification = Justify.Left
        };
}