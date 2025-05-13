using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.Controllers;

public class ContactController
{
    public void Add()
    {
        AnsiConsole.Clear();
        
        var name = PromptForString("Name", Contact.MaxNameLength);
        
        var email = PromptForString("E-mail address", Contact.MaxEmailLength, true);
        
        var phone = PromptForString("Phone number", Contact.MaxPhoneLength, true);
        
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

    // TODO probably need to move to helpers?
    private static string PromptForString(string fieldName, int maxLength, bool optional = false)
    {
        AnsiConsole.Write(Helpers.GetStandardRule(fieldName));
        
        var prefix = optional ? "[[Optional]] " : "";
        
        var prompt = new TextPrompt<string>($"{prefix}What [green]{fieldName.ToLower()}[/] would you like to use?")
            {
                AllowEmpty = optional
            }
            .Validate((input) =>
            {
                if (string.IsNullOrWhiteSpace(fieldName))
                    return ValidationResult.Error($"[red]Cannot use empty {fieldName.ToLower()}[/]");
                
                return input.Length > maxLength 
                    ? ValidationResult.Error($"[red]{fieldName.ToLower()} length must be less than {maxLength}[/]") 
                    : ValidationResult.Success();
            });
        
        return AnsiConsole.Prompt(prompt);
    }
}