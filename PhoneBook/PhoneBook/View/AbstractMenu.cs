using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

public abstract class AbstractMenu
{
    protected static T Prompt<T>() where T : struct, Enum =>
        AnsiConsole.Prompt(new SelectionPrompt<T>()
            .Title("What would you like to do?")
            .AddChoices(Enum.GetValues<T>())
            .UseConverter(Helpers.GetEnumDisplayValue));
}