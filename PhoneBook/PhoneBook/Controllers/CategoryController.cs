using Microsoft.EntityFrameworkCore;
using PhoneBook.Model;
using Spectre.Console;
using static PhoneBook.Util.Helpers;
using static PhoneBook.Model.Category;

namespace PhoneBook.Controllers;

public class CategoryController
{
    public void Add()
    {
        AnsiConsole.Clear();

        var name = PromptForString("Name", ValidateName);
        
        using var db = new ContactContext();
        var category = db.Categories.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if (category != null)
        {
            AnsiConsole.MarkupLine($"[red]Category with name '{name}' already exists![/]");
            return;
        }
        
        db.Add(new Category
        {
            Name = name
        });
        db.SaveChanges();
    }

    public void Delete(Category category)
    {
        using var db = new ContactContext();
        try
        {
            var contacts = db.Contacts.Where(x => x.CategoryId == category.Id).ToList();

            foreach (var contact in contacts)
            {
                contact.CategoryId = null;
                contact.Category = null;
            }
            
            db.Categories.Remove(category);
            db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            AnsiConsole.Markup($"[red]{category.Name}[/] has already been deleted");
        }
    }
    
    public List<Category> List(bool includeContacts = false)
    {
        using var db = new ContactContext();
        
        var categories = db.Categories.AsQueryable()
            .OrderBy(c => c.Name);
        
        if (includeContacts)
            return categories.Include(x => x.Contacts).ToList();
        
        return categories.ToList();
    }
}