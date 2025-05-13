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
        AnsiConsole.Clear();
        
        using var db = new ContactContext();
        var entries = db.Contacts.AsQueryable();

        if (AnsiConsole.Confirm("Query by name?"))
        {
            var nameSearch = PromptForQueryString("Name");
            entries = entries.Where(c => c.Name.ToLower().Contains(nameSearch));
        }

        if (AnsiConsole.Confirm("Query by email?"))
        {
            var emailSearch = PromptForQueryString("E-mail");
            entries = entries.Where(c => !string.IsNullOrWhiteSpace(c.Email) 
                                         && c.Email.ToLower().Contains(emailSearch));
        }

        if (AnsiConsole.Confirm("Query by number?"))
        {
            var phoneNumberSearch = PromptForQueryString("Phone");
            entries = entries.Where(c => !string.IsNullOrWhiteSpace(c.PhoneNumber)  
                                        && c.PhoneNumber.Contains(phoneNumberSearch));
        }
            
        AnsiConsole.Clear();
        
        var table = new Table();
        
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Email"));
        table.AddColumn(new TableColumn("Phone"));

        foreach (var entry in entries)
        {
            table.AddRow(entry.Name, entry.Email ?? string.Empty, entry.PhoneNumber ?? string.Empty);
        }
        
        AnsiConsole.Write(table);
        
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
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

    private static string PromptForQueryString(string fieldName)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(Helpers.GetStandardRule("Name"));

        return AnsiConsole.Prompt(new TextPrompt<string>("Name [green]contains[/]:")).ToLower();
    }
}