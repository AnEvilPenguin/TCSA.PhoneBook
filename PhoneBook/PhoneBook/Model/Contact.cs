using System.ComponentModel.DataAnnotations;

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
}