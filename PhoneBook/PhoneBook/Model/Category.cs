using System.ComponentModel.DataAnnotations;
using ValidationResult = Spectre.Console.ValidationResult;

namespace PhoneBook.Model;

public class Category
{
    public const int MaxNameLength = 70;
    
    public int Id { get; init; }
    
    [MaxLength(MaxNameLength)]
    public required string Name { get; set; }
    
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public static ValidationResult ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValidationResult.Error($"[red]Cannot use empty name[/]");
                
        return name.Length > MaxNameLength 
            ? ValidationResult.Error($"[red]name length must be less than {MaxNameLength}[/]") 
            : ValidationResult.Success();
    }
}