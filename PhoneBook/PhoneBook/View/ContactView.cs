using System.Reflection;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

public static class ContactView
{
    public static void ContactTable(List<Contact> contacts)
    {
        AnsiConsole.Clear();
        
        var table = new Table();
        
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Email"));
        table.AddColumn(new TableColumn("Phone"));
        
        var sampleSize = Math.Min(contacts.Count, 8);

        foreach (var entry in contacts.GetRange(0, sampleSize))
        {
            table.AddRow(entry.Name, entry.Email ?? string.Empty, entry.PhoneNumber ?? string.Empty);
        }
        
        AnsiConsole.Write(table);
        
        if (contacts.Count > 10)
            AnsiConsole.MarkupLine($"Omitted [yellow]{contacts.Count - 10}[/] contacts from the table...");
    }

    public static PropertyInfo PromptForProperty(string prompt)
    {
        var stringProperties = typeof(Contact)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(string));
        
        return AnsiConsole.Prompt(new SelectionPrompt<PropertyInfo>()
            .Title("What property would you like to search against?")
            .AddChoices(stringProperties)
            .UseConverter(info => info.Name));
    }
    
    public static string PromptForString(string fieldName, Func<string, ValidationResult> validator, bool optional = false)
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