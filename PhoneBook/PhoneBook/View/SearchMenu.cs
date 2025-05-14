using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using PhoneBook.Model;
using Spectre.Console;
using static PhoneBook.View.ContactView;

namespace PhoneBook.View;

enum SearchOption
{
    [Display(Name = "New Search")]
    Search,
    [Display(Name = "Select Contact")]
    Select,
    [Display(Name = "Back to Main Menu")]
    Back
}

public class SearchMenu(ContactController contactController) : AbstractMenu
{
    private readonly ContactMenu _contactMenu = new(contactController);
    
    public void Run()
    {
        var contacts = RunSearch();
        
        SearchOption? choice = null;

        while (choice != SearchOption.Back)
        {
            
            choice = Prompt<SearchOption>();
            
            switch (choice)
            {
                case SearchOption.Search:
                    contacts = RunSearch();
                    break;
                
                case SearchOption.Select:
                    if (contacts.Any())
                    {
                        SelectContact(contacts);
                        contacts.Clear();
                        break;
                    }

                    AnsiConsole.MarkupLine("[red]Error: No contacts found.[/]");
                    AnsiConsole.MarkupLine("Press any key to continue...");
                    Console.ReadKey(true);
                    AnsiConsole.Clear();
                    break;
            }
        }
    }

    private List<Contact> RunSearch()
    {
        var contacts = contactController.Search();
        
        ContactTable(contacts);
        
        return contacts;
    }

    private void SelectContact(List<Contact> contacts)
    {
        var contact = contacts.Count == 1
            ? contacts[0]
            : AnsiConsole.Prompt(new SelectionPrompt<Contact>()
                .Title("Select contact:")
                .AddChoices(contacts)
                .UseConverter(c => c.ToString()));
            
        
        _contactMenu.Run(contact);
    }
}