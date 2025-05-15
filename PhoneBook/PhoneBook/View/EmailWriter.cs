using System.Text;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.View;

public static class EmailWriter
{
    public static Email? NewEmailMessage()
    {
        string? subject = null;
        
        AnsiConsole.Write(Helpers.GetStandardRule("Subject"));
        
        if (AnsiConsole.Confirm("Would you like to add a subject?"))
            subject = AnsiConsole.Ask<string>("Subject:");

        AnsiConsole.Write(Helpers.GetStandardRule("Body"));

        var body = new StringBuilder();

        do
        {
            body.AppendLine(AnsiConsole.Ask<string>("Add to your body:"));
        } while (AnsiConsole.Confirm("Would you like to add another line?"));

        AnsiConsole.Clear();
        
        AnsiConsole.WriteLine($"Subject: {subject}");
        AnsiConsole.WriteLine($"Body: {body}");
        
        if (AnsiConsole.Confirm("Would you like to send this Email?"))
            return new Email { Subject = subject, Body = body.ToString() };
        
        return null;
    }
}