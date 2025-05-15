using System.Reflection;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;
using static PhoneBook.View.ContactView;

namespace PhoneBook.View;


public static class QueryBuilder
{
    public static IQueryable<Contact> SimpleQuery(IQueryable<Contact> contacts, List<Category> categories)
    {
        AnsiConsole.Write(Helpers.GetStandardRule("Simple Query"));

        var property = PromptForProperty("What property would you like to search against?", categories.Any());
        
        AnsiConsole.Write(Helpers.GetStandardRule(property));


        if (property == "Category")
        {
            var category = AnsiConsole.Prompt(new SelectionPrompt<Category>()
                .Title("What category?")
                .AddChoices(categories)
                .UseConverter(c => c.Name));
            
            return contacts.Where(c => c.CategoryId == category.Id);
        }
        var queryString = AnsiConsole.Prompt(new TextPrompt<string>($"{property} [green]contains[/]:"))
            .ToLower();

        return property switch
        {
            "PhoneNumber" => contacts.Where(c => !string.IsNullOrWhiteSpace(c.PhoneNumber) 
                                                 && c.PhoneNumber.Contains(queryString)),
            "Email" => contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) 
                                           && c.Email.Contains(queryString)),
            _ => contacts.Where(c => c.Name.ToLower().Contains(queryString))
        };
    }
}