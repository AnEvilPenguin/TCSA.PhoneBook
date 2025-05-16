using System.Net.Http.Headers;
using PhoneBook.Model;
using PhoneBook.Util;

namespace PhoneBook.Controllers;

public class SmsController
{
    private TwilioSettings? twilioSettings = Configuration.GetTwilioSettings();

    public bool CanSendSms =>
        twilioSettings != null &&
        !string.IsNullOrWhiteSpace(twilioSettings.Sid) && 
        !string.IsNullOrWhiteSpace(twilioSettings.Token) && 
        !string.IsNullOrWhiteSpace(twilioSettings.Number);

    public async Task SendSmsAsync(Contact contact, string message)
    {
        var authString = $"{twilioSettings!.Sid}:{twilioSettings.Token}";
        var base64AuthString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));

        using var client = new HttpClient();
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);

        var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
                {
                    {"To", contact.PhoneNumber!.ToString()},
                    {"From", twilioSettings!.Number!.ToString()},
                    {"Body", message}
                });
        
        await client.PostAsync(new Uri($"https://api.twilio.com/2010-04-01/Accounts/{twilioSettings!.Sid}/Messages.json"), content);
    }
}