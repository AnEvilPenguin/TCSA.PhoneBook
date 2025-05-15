using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

enum MainMenuOption
{
    [Display(Name = "Search Contacts")]
    Search,
    [Display(Name = "New Contact")]
    Add,
    [Display(Name = "Manage Categories")]
    Categories,
    [Display(Name = "Quit")]
    Quit
}

public class MainMenu : AbstractMenu
{
    private readonly ContactController _contactController;
    private readonly SearchMenu _searchMenu;
    private readonly CategoryMenu _categoryMenu;

    public MainMenu()
    {
        _contactController = new ContactController();
        _searchMenu = new SearchMenu(_contactController);
        _categoryMenu = new CategoryMenu();
    }
    
    public int Run()
    {
        MainMenuOption? choice = null;

        while (choice != MainMenuOption.Quit)
        {
            AnsiConsole.Clear();

            choice = Prompt<MainMenuOption>();

            switch (choice)
            {
                case MainMenuOption.Add:
                    _contactController.Add();
                    break;
                
                case MainMenuOption.Search:
                    _searchMenu.Run();
                    break;
                
                case MainMenuOption.Categories:
                    _categoryMenu.Run();
                    break;
            }
        }
        
        return 0;
    }
}