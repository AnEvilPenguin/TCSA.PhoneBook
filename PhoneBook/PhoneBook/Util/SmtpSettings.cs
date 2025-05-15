namespace PhoneBook.Util;

public class SmtpSettings
{
    public required Uri Uri { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public required string SenderName { get; set; }
    public required string From { get; set; }
}