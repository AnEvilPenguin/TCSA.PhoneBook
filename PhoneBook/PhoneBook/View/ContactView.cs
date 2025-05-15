using System.Reflection;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

public static class ContactView
{
    private const int TableLimit = 8;
    
    public static void ContactTable(List<Contact> contacts)
    {
        AnsiConsole.Clear();

        var table = new Table();

        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Email"));
        table.AddColumn(new TableColumn("Phone"));
        table.AddColumn(new TableColumn("Category"));

        var sampleSize = Math.Min(contacts.Count, TableLimit);

        foreach (var entry in contacts.GetRange(0, sampleSize))
        {
            table.AddRow(entry.Name, 
                entry.Email ?? string.Empty, 
                entry.PhoneNumber ?? string.Empty,
                entry.Category?.Name ?? string.Empty);
        }

        AnsiConsole.Write(table);

        if (contacts.Count > TableLimit)
            AnsiConsole.MarkupLine($"Omitted [yellow]{contacts.Count - TableLimit}[/] contacts from the table...");
    }

    public static string PromptForProperty(string prompt, bool includeCategory = false)
    {
        var stringProperties = typeof(Contact)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .Select(x => x.Name);
        
        if (includeCategory)
            stringProperties = stringProperties.Append("Category");

        return AnsiConsole.Prompt<string>(new SelectionPrompt<string>()
            .Title("What property would you like to search against?")
            .AddChoices(stringProperties));
    }
}