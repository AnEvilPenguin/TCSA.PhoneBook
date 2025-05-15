using System.ComponentModel.DataAnnotations;
using PhoneBook.Controllers;
using PhoneBook.Model;
using Spectre.Console;
using static PhoneBook.View.ContactView;

namespace PhoneBook.View;

enum ContactOption
{
    [Display(Name = "Update Contact")]
    Update,
    [Display(Name = "Delete Contact")]
    Delete,
    [Display(Name = "Back to Search Menu")]
    Back
}

public class ContactMenu (ContactController contactController) : AbstractMenu
{
    public void Run(Contact contact)
    {
        ContactOption? choice = null;
        
        while (choice != ContactOption.Back)
        {
            ContactTable([contact]);
            
            choice = Prompt<ContactOption>();
            
            switch (choice)
            {
                case ContactOption.Update:
                    contact = contactController.Update(contact);
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