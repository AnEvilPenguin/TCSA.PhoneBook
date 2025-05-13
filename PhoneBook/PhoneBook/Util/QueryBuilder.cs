using PhoneBook.Model;
using Spectre.Console;

namespace PhoneBook.Util;

public class QueryBuilder
{
    private IQueryable<Contact> _contacts;

    public QueryBuilder(IQueryable<Contact> contacts)
    {
        _contacts = contacts;
    }
    
    // TODO Do this properly
    // Some sort of menu where we can pick properties
    // select the type of operation
    // eq, ne, contains, notcontains, etc.
    // some sort of UI to show where we are and what is going on
    // add Condition, remove condition, search, abort (etc?)

    public void NameQuery()
    {
        if (AnsiConsole.Confirm("Query by name?"))
        {
            var nameSearch = PromptForQueryString("Name");
            _contacts = _contacts.Where(c => c.Name.ToLower().Contains(nameSearch));
        }
    }

    public void EmailQuery()
    {
        if (AnsiConsole.Confirm("Query by email?"))
        {
            var emailSearch = PromptForQueryString("E-mail");
            _contacts = _contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) 
                                         && c.Email.ToLower().Contains(emailSearch));
        }
    }

    public void NumberQuery()
    {
        if (AnsiConsole.Confirm("Query by number?"))
        {
            var phoneNumberSearch = PromptForQueryString("Phone");
            _contacts = _contacts.Where(c => !string.IsNullOrWhiteSpace(c.PhoneNumber)  
                                         && c.PhoneNumber.Contains(phoneNumberSearch));
        }
    }

    public IQueryable<Contact> GetQuery() =>
        _contacts;
    
    private static string PromptForQueryString(string fieldName)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(Helpers.GetStandardRule("Name"));

        return AnsiConsole.Prompt(new TextPrompt<string>("Name [green]contains[/]:")).ToLower();
    }
}