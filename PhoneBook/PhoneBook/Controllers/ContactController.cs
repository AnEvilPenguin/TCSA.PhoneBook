using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;
using static PhoneBook.Model.Contact;
using static PhoneBook.View.ContactView;

namespace PhoneBook.Controllers;

public class ContactController
{
    public void Add()
    {
        AnsiConsole.Clear();
        
        var name = PromptForString("Name", ValidateName);
        
        var email = PromptForString("E-mail address", ValidateEmail, true);
        
        var phone = PromptForString("Phone number", ValidatePhoneNumber, true);
        
        using var db = new ContactContext();
        db.Add(new Contact
        {
            Name = name,
            Email = email,
            PhoneNumber = phone
        });
        db.SaveChanges();
    }

    public void Search()
    {
        AnsiConsole.Clear();
        
        using var db = new ContactContext();
        var entries = db.Contacts.AsQueryable();
        
        var queryBuilder = new QueryBuilder(entries);
        
        queryBuilder.NameQuery();
        queryBuilder.EmailQuery();
        queryBuilder.NumberQuery();

        entries = queryBuilder.GetQuery();
            
        ContactTable(entries);
    }
    
    private static string PromptForString(string fieldName, Func<string, ValidationResult> validator, bool optional = false)
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