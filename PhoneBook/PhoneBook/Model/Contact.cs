using System.ComponentModel.DataAnnotations;
using ValidationResult = Spectre.Console.ValidationResult;
using PhoneNumbers;

namespace PhoneBook.Model;

public class Contact
{
    // MaxLength following NHS guidance for UI
    // https://www.datadictionary.nhs.uk/data_elements/person_full_name.html
    public const int MaxNameLength = 70;
    // Follows RFC 5321
    public const int MaxEmailLength = 254;
    // Winged it as people like whitespace
    public const int MaxPhoneLength = 20;
    
    public int Id { get; init; }

    [MaxLength(MaxNameLength)]
    public required string Name { get; set; }
    
    [MaxLength(MaxEmailLength)]
    public string? Email { get; set; }
    
    [MaxLength(MaxPhoneLength)]
    public string? PhoneNumber { get; set; }

    public static ValidationResult ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValidationResult.Error($"[red]Cannot use empty name[/]");
                
        return name.Length > MaxNameLength 
            ? ValidationResult.Error($"[red]name length must be less than {MaxNameLength}[/]") 
            : ValidationResult.Success();
    }

    public static ValidationResult ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return ValidationResult.Success();
        
        var trimmedEmail = email.Trim();
        
        if (trimmedEmail.Length > MaxEmailLength)
            return ValidationResult.Error($"[red]email length must be less than {MaxEmailLength}[/]");
        
        if (trimmedEmail.EndsWith('.'))
            return ValidationResult.Error($"[red]Invalid email[/]");

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);

            return addr.Address == trimmedEmail
                ? ValidationResult.Success()
                : ValidationResult.Error($"[red]Invalid email[/]");
        }
        catch (FormatException formatException)
        {
            return ValidationResult.Error($"[red]Invalid email: {formatException.Message}[/]");
        }
        catch (Exception e)
        {
            return ValidationResult.Error($"[red]Invalid email: {e.Message}[/]");
        }
    }

    public static ValidationResult ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return ValidationResult.Success();
        
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();

        PhoneNumber? parsed = null;

        try
        {
            parsed = phoneNumberUtil.Parse(phoneNumber,"GB");
        }
        catch (Exception e)
        {
            return ValidationResult.Error($"[red]Invalid number: {e.Message}[/]");
        }
        
        return phoneNumberUtil.IsValidNumber(parsed) 
            ? ValidationResult.Success() 
            : ValidationResult.Error($"[red]Invalid number[/]");
    }
}