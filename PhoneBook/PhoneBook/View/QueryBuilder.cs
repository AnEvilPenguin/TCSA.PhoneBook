using System.Reflection;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

/*
public enum QueryType
{
    Simple,
    Complex
}
*/

public static class QueryBuilder
{
    // I've run into some issues here. Mainly getting EF to build a query from my dynamic source.
    // Reverting to keeping things simple and then maybe come back later
    /*
    public static IQueryable<Contact> Prompt(IQueryable<Contact> contacts)
    {
        AnsiConsole.Write(Helpers.GetStandardRule("Query Type"));
        
        var choice = AnsiConsole.Prompt(new SelectionPrompt<QueryType>()
            .Title("What type of query would you like to run?")
            .AddChoices(Enum.GetValues<QueryType>())
            .UseConverter(Helpers.GetEnumDisplayValue));

        return choice == QueryType.Simple
            ? SimpleQuery(contacts)
            : ComplexQuery(contacts);
    }
    */

    public static IQueryable<Contact> SimpleQuery(IQueryable<Contact> contacts)
    {
        AnsiConsole.Write(Helpers.GetStandardRule("Simple Query"));
        
        var stringProperties = typeof(Contact)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(string));

        var property = AnsiConsole.Prompt(new SelectionPrompt<PropertyInfo>()
            .Title("What property would you like to search against?")
            .AddChoices(stringProperties)
            .UseConverter(info => info.Name));
        
        AnsiConsole.Write(Helpers.GetStandardRule(property.Name));
        
        var queryString = AnsiConsole.Prompt(new TextPrompt<string>($"{property.Name} [green]contains[/]:"))
            .ToLower();

        return property.Name switch
        {
            "PhoneNumber" => contacts.Where(c => !string.IsNullOrWhiteSpace(c.PhoneNumber) 
                                                 && c.PhoneNumber.Contains(queryString)),
            "Email" => contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) 
                                           && c.Email.Contains(queryString)),
            _ => contacts.Where(c => c.Name.ToLower().Contains(queryString))
        };
    }

    /*
    private static IQueryable<Contact> ComplexQuery(IQueryable<Contact> contacts)
    {
        return contacts;
    }
    */
}