using System.ComponentModel.DataAnnotations;
using Spectre.Console;

namespace PhoneBook.View;

enum MainMenuOption
{
    [Display(Name = "Search Contacts")]
    Search,
    [Display(Name = "Add New Contact")]
    Add,
    [Display(Name = "Edit Contact")]
    Update,
    [Display(Name = "Delete Contact")]
    Delete,
    [Display(Name = "Quit")]
    Quit
}

public class MainMenu
{
    public int Run()
    {
        MainMenuOption? choice = null;

        while (choice != MainMenuOption.Quit)
        {
            AnsiConsole.Clear();

            choice = Prompt();
        }
        
        return 0;
    }

    private MainMenuOption Prompt() => 
        AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>()
            .Title("What would you like to do?")
            .AddChoices(Enum.GetValues<MainMenuOption>())
            .UseConverter(Helpers.GetEnumDisplayValue));
}