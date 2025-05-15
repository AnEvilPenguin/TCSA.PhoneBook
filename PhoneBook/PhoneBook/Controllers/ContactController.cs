using Microsoft.EntityFrameworkCore;
using PhoneBook.Model;
using PhoneBook.View;
using Spectre.Console;
using static PhoneBook.Model.Contact;
using static PhoneBook.View.ContactView;

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
        
        using var db = new ContactContext();
        var entries = db.Contacts.AsQueryable();
        
        entries = QueryBuilder.SimpleQuery(entries)
            .OrderBy(c => c.Name);
        
        return entries.ToList();
    }

    public Contact Update(Contact contact)
    {
        var property = PromptForProperty("What property do you want to update?");

        switch (property.Name)
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
        }
        
        using var db = new ContactContext();
        db.Update(contact);
        db.SaveChanges();
        
        return contact;
    }
}