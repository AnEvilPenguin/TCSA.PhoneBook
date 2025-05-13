using Microsoft.EntityFrameworkCore;
using PhoneBook.Model;
using PhoneBook.Util;

namespace PhoneBook.Controllers;

public class ContactContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionSettings = Configuration.GetConnectionSettings();
        
        if (connectionSettings == null)
            throw new Exception("No connection settings found");
        
        optionsBuilder.UseSqlServer(connectionSettings.ConnectionString);
    }
        
}