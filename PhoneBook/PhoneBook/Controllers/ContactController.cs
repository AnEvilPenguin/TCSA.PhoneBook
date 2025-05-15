using Microsoft.EntityFrameworkCore;
using PhoneBook.Model;
using PhoneBook.View;
using Spectre.Console;
using static PhoneBook.Model.Contact;
using static PhoneBook.View.ContactView;
using static PhoneBook.Util.Helpers;

namespace PhoneBook.Controllers;

public class ContactController
{
    private CategoryController _categoryController = new ();
    
    public void Add()
    {
        AnsiConsole.Clear();
        
        var name = PromptForString("Name", ValidateName);
        
        var email = PromptForString("E-mail address", ValidateEmail, true);
        
        var phone = PromptForString("Phone number", ValidatePhoneNumber, true);
        
        var categories = _categoryController.List();
        
        Category? category = null;

        if (categories.Any() && AnsiConsole.Confirm("Add category?"))
            category = AnsiConsole.Prompt(new SelectionPrompt<Category>()
                .Title("What category would you like to add?")
                .AddChoices(categories)
                .UseConverter(c => c.Name));
        
        using var db = new ContactContext();
        db.Add(new Contact
        {
            Name = name,
            Email = email,
            PhoneNumber = phone,
            CategoryId = category?.Id
        });
        db.SaveChanges();
    }

    public void Delete(Contact contact)
    {
        
        using var db = new ContactContext();
        try
        {
            db.Remove(contact);
            db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            AnsiConsole.Markup($"[red]{contact.Name}[/] has already been deleted");
        }
        
    }

    public List<Contact> Search()
    {
        AnsiConsole.Clear();
        
        var categories = _categoryController.List();
        
        using var db = new ContactContext();
        var entries = db.Contacts.AsQueryable();
        
        entries = QueryBuilder.SimpleQuery(entries, categories)
            .OrderBy(c => c.Name);
        
        return entries.Include(c => c.Category).ToList();
    }

    public Contact Update(Contact contact)
    {
        var categories = _categoryController.List();
        
        var property = PromptForProperty("What property do you want to update?", categories.Any());

        switch (property)
        {
            case "Name":
                contact.Name = PromptForString("Name", ValidateName);
                break;
            
            case "Email":
                contact.Email = PromptForString("E-mail address", ValidateEmail, true);
                break;
            
            case "PhoneNumber":
                contact.PhoneNumber = PromptForString("Phone number", ValidatePhoneNumber, true);
                break;
            
            case "Category":
                var category = AnsiConsole.Prompt(new SelectionPrompt<Category>()
                    .Title("What category would you like to use?")
                    .AddChoices(categories)
                    .UseConverter(c => c.Name));
                contact.Category = category;
                contact.CategoryId = category.Id;
                break;
        }
        
        using var db = new ContactContext();
        db.Update(contact);
        db.SaveChanges();
        
        return contact;
    }
}