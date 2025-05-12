using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PhoneBook;

public class ProductContext : DbContext
{
    private readonly ConnectionSettings? _connectionSettings;
    
    public ProductContext()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddEnvironmentVariables()
            .Build();


        _connectionSettings = config.GetSection("ConnectionSettings").Get<ConnectionSettings>();
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(_connectionSettings?.ConnectionString);
}