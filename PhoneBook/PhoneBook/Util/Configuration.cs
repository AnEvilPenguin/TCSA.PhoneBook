using Microsoft.Extensions.Configuration;

namespace PhoneBook.Util;

public static class Configuration
{
    private static ConnectionSettings? _connectionSettings;
    private static TwilioSettings? _twilioSettings;
    private static SmtpSettings? _smtpSettings;
    
    private static IConfigurationRoot _configuration = new ConfigurationBuilder()
        .AddJsonFile("appSettings.json")
        .Build();

    public static ConnectionSettings? GetConnectionSettings()
    {
        if (_connectionSettings != null) 
            return _connectionSettings;
        
        _connectionSettings = _configuration.GetRequiredSection("ConnectionSettings").Get<ConnectionSettings>();

        return _connectionSettings;
    }

    public static TwilioSettings? GetTwilioSettings()
    {
        if (_twilioSettings != null)
            return _twilioSettings;
        
        _twilioSettings = _configuration.GetSection("TwilioSettings").Get<TwilioSettings>();
        
        return _twilioSettings;
    }
    
    public static SmtpSettings? GetSmtpSettings()
    {
        if (_smtpSettings != null)
            return _smtpSettings;
        
        _smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
        
        return _smtpSettings;
    }
}