using Microsoft.Extensions.Configuration;

namespace PhoneBook.Util;

public static class Configuration
{
    private static ConnectionSettings? _connectionSettings;

    public static ConnectionSettings? GetConnectionSettings()
    {
        if (_connectionSettings != null) 
            return _connectionSettings;
        
        var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .Build();
            
        _connectionSettings = config.GetSection("ConnectionSettings").Get<ConnectionSettings>();

        return _connectionSettings;
    }
}