using PhoneBook.Model;
using PhoneBook.Util;

namespace PhoneBook.Controllers;

public class SmsController
{
    private TwilioSettings? twilioSettings = Configuration.GetTwilioSettings();

    public bool CanSendSms =>
        twilioSettings != null &&
        !string.IsNullOrWhiteSpace(twilioSettings.TwilioSid) && 
        !string.IsNullOrWhiteSpace(twilioSettings.TwilioToken) && 
        !string.IsNullOrWhiteSpace(twilioSettings.TwilioNumber);

    public Task SendSmsAsync(Contact contact)
    {
        // Waiting on twilio to deal with regulatory stuff before I can get a number to test with
        throw new NotImplementedException();
    }
}