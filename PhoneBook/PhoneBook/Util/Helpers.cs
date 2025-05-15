using System.ComponentModel.DataAnnotations;
using Spectre.Console;
using ValidationResult = Spectre.Console.ValidationResult;

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
    
    public static string PromptForString(string fieldName, Func<string, ValidationResult> validator, bool optional = false)
    {
        AnsiConsole.Write(Helpers.GetStandardRule(fieldName));
        
        var prefix = optional ? "[[Optional]] " : "";
        
        var prompt = new TextPrompt<string>($"{prefix}What [green]{fieldName.ToLower()}[/] would you like to use?")
            {
                AllowEmpty = optional
            }
            .Validate(validator);
        
        return AnsiConsole.Prompt(prompt);
    }
}