using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Model;

public class Contact
{
    public int Id { get; set; }
    // MaxLength following NHS guidance for UI
    // https://www.datadictionary.nhs.uk/data_elements/person_full_name.html
    [MaxLength(70)]
    public required string Name { get; set; }
    // Follows RFC 5321
    [MaxLength(254)]
    public string? Email { get; set; }
    // Winged it as people like whitespace
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
}