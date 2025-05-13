using PhoneBook.Model;
using Spectre.Console;

namespace PhoneBook.View;

public static class ContactView
{
    public static void ContactTable(IQueryable<Contact> contacts)
    {
        AnsiConsole.Clear();
        
        var table = new Table();
        
        table.AddColumn(new TableColumn("Name"));
        table.AddColumn(new TableColumn("Email"));
        table.AddColumn(new TableColumn("Phone"));

        foreach (var entry in contacts)
        {
            table.AddRow(entry.Name, entry.Email ?? string.Empty, entry.PhoneNumber ?? string.Empty);
        }
        
        AnsiConsole.Write(table);
        
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}