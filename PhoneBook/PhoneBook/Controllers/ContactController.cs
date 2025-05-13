using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;
using static PhoneBook.Model.Contact;

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
        throw new NotImplementedException();
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