using System.Reflection;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;


public static class QueryBuilder
{
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
}