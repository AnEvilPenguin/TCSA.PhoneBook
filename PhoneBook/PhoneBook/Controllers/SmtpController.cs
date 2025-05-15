using MailKit.Net.Smtp;
using MimeKit;
using PhoneBook.Model;
using PhoneBook.Util;
using Spectre.Console;

namespace PhoneBook.Controllers;

public class SmtpController
{
    private SmtpSettings? _smtpSettings = Configuration.GetSmtpSettings();
    
    public bool CanSendEmail =>
        _smtpSettings != null;

    public void SendEmail(Contact contact, Email email)
    {
        if (_smtpSettings == null)
            throw new Exception("Can not send email");
        
        var message = GenerateMessage(contact, email);
        
        using var client = new SmtpClient();

        try
        {
            client.Connect(_smtpSettings.Uri);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Failed to connect to server with error: {ex.Message}");
            return;
        }

        if (_smtpSettings.Username != null && _smtpSettings.Password != null)
            client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);

        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Failed to send message with error: {ex.Message}");
        }
        finally
        {
            client.Disconnect(true);
        }
    }
    
    private MimeMessage GenerateMessage(Contact contact, Email email)
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(_smtpSettings!.SenderName, _smtpSettings.From));
        message.To.Add(new MailboxAddress(contact.Name, contact.Email));
        
        message.Subject = email.Subject;
        message.Body = new TextPart("plain")
        {
            Text = email.Body
        };
        
        return message;
    }
}