using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using Spectre.Console;

namespace PhoneBook.View;

enum MainMenuOption
{
    [Display(Name = "Search Contacts")]
    Search,
    [Display(Name = "New Contact")]
    Add,
    [Display(Name = "Quit")]
    Quit
}

public class MainMenu
{
    private readonly ContactController _contactController = new ContactController();
    
    public int Run()
    {
        MainMenuOption? choice = null;

        while (choice != MainMenuOption.Quit)
        {
            AnsiConsole.Clear();

            choice = Prompt();

            switch (choice)
            {
                case MainMenuOption.Add:
                    _contactController.Add();
                    break;
                
                case MainMenuOption.Search:
                    _contactController.Search();
                    break;
            }
        }
        
        return 0;
    }

    private MainMenuOption Prompt() => 
        AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>()
            .Title("What would you like to do?")
            .AddChoices(Enum.GetValues<MainMenuOption>())
            .UseConverter(Helpers.GetEnumDisplayValue));
}