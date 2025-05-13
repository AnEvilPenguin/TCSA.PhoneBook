using System.ComponentModel.DataAnnotations;

namespace PhoneBook;

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
}