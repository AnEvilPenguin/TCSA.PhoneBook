using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;
using static PhoneBook.View.ContactView;

namespace PhoneBook.View;

public enum ContactOption
{
    Update,
    Delete,
    Sms,
    Email,
    Back
}

public class ContactOperation
{
    public string Name { get; set; }
    public ContactOption Option { get; set; }
}

public class ContactMenu (ContactController contactController) : AbstractMenu
{
    private readonly List<ContactOperation> _operations = [
        new ContactOperation() { Name = "Update Contact", Option = ContactOption.Update, },
        new ContactOperation() { Name = "Delete Contact", Option = ContactOption.Delete, },
        new ContactOperation() { Name = "Back to Search Menu", Option = ContactOption.Back, }
    ];
    
    private readonly SmsController _smsController = new();
    private readonly SmtpController _smtpController = new();
    
    public void Run(Contact contact)
    {
        ContactOperation? choice = null;
        
        while (choice?.Option != ContactOption.Back)
        {
            ContactTable([contact]);
            
            var options = new List<ContactOperation>(_operations);
            
            if (_smsController.CanSendSms && contact.PhoneNumber != null)
                options.Insert(2, new ContactOperation() { Name = "Send SMS", Option = ContactOption.Sms, });
            
            if (_smtpController.CanSendEmail && contact.Email != null)
                options.Insert(2, new ContactOperation() { Name = "Send Email", Option = ContactOption.Email, });
            
            choice = AnsiConsole.Prompt(new SelectionPrompt<ContactOperation>()
                .Title("What would you like to do?")
                .AddChoices(options)
                .UseConverter(o => o.Name));
            
            switch (choice.Option)
            {
                case ContactOption.Update:
                    contact = contactController.Update(contact);
                    break;
                
                case ContactOption.Sms:
                    
                    break;
                
                case ContactOption.Email:
                    var message = EmailWriter.NewEmailMessage();
                    
                    if (message != null)
                        _smtpController.SendEmail(contact, message);
                    
                    break;
                
                case ContactOption.Delete:
                    contactController.Delete(contact);
                    AnsiConsole.Clear();
                    return;
            }
        }
        
        AnsiConsole.Clear();
    }
}