using Microsoft.EntityFrameworkCore;
using PhoneBook.Model;
using PhoneBook.Util;

namespace PhoneBook.Controllers;

public class ContactContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionSettings = Configuration.GetConnectionSettings();
        
        if (connectionSettings == null)
            throw new Exception("No connection settings found");
        
        optionsBuilder.UseSqlServer(connectionSettings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasIndex(c => new { c.Name })
            .IsUnique(true);

        modelBuilder.Entity<Category>()
            .HasMany(e => e.Contacts)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId)
            .HasPrincipalKey(e => e.Id);
    }
}