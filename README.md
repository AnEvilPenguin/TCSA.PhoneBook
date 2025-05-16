# Phone Book

This is a project primarily based around starting to learn Entity Framework.  
This is a very simple 'phone book'. For those of you not old enough to know what this is, once upon a time every family
had a communal telephone that could be used to call other families. But, usually, most of the people you called were not 
important enough to you to actually bother memorizing their telephone number, so you would have a little notebook beside 
the phone (or in a drawer if you were fancy) that you could use to write down names and numbers in a semi-legible 
fashion.

# How to use

To begin with you will need an instance of Microsoft SQL Server. There shouldn't be any requirement to use a specific 
type or edition of SQL Server, however this was developed using the Linux docker image:

``` bash
docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

Which will then run the container using the default port, the developer edition, and (at the time of writing) the 2022 
version of the software.

Then you will need to run the [setup.sql](/PhoneBook/setup.sql) script to prepare the database.

Build the project however you normally would. E.g:

```bash
dotnet build ./PhoneBook/PhoneBook.sln
```

You will next need to update the appSettings.json file with your database connection string.  
It should looks something like:

```json
{
  "ConnectionSettings": {
    "ConnectionString": "Server=localhost;Database=Contacts;User Id=myUser;Password=myPass;TrustServerCertificate=true;"
  }
}
```

If you would like to use the optional email sending functionality you will need to add SmtpSettings:

```json
{
  "ConnectionSettings": {
    "ConnectionString": "Server=localhost;Database=Contacts;User Id=myUser;Password=myPass;TrustServerCertificate=true;"
  },
  "SmtpSettings": {
      "Uri": "smtps://smtp.gmail.com:465",
      "Username": "feathers@gmail.com",
      "Password": "superSecurePassword",
      "SenderName": "Feathers McGraw",
      "From": "feathers@gmail.com"
    }
}
```
If using Gmail, you may need to generate an app password. Otherwise check with your provider.

To use Twilio fill in your AccountSid, Token, and phone number:

```json
{
  "ConnectionSettings": {
    "ConnectionString": "Server=localhost;Database=Contacts;User Id=sa;Password=Twstlb4e;TrustServerCertificate=true;"
  },
  "TwilioSettings": {
    "Sid": "ABC12345...",
    "Token": "a1b23c...",
    "Number": "+4477123456"
  }
}

```

When you run the executable you'll be presented with a the main menu:

![Main menu](/Docs/Main_menu.png)

Use the 'New Contact' menu to create a contact.  
To work with existing contacts use the 'Search Contacts' menu.

![Search menu](/Docs/Search.png)

You will be able to Update or Delete the contact. If you have SMTP configured and the user has an email address, you
will also be able to send a basic email to the user. If you have Twilio configured and the user has a phone number, you 
will be able to send an SMS to the contact.

To manage categories, use the 'Manage Categories' menu item.

![Categories menu](/Docs/Categories.png)

You'll be able to create, rename or delete any categories.

# Requirements

- [X] Records contacts and their phone numbers
- [X] Console based
- [X] Should be CRUD
- [X] Need to use Entity Framework
  - [X] CANNOT use raw SQL
- [X] Should have a base contact class with the following minimum properties
  - [X] Id INT
  - [X] Name STRING
  - [X] Email STRING
  - [X] Phone Number STRING
- [X] Validation
  - [X] Email address
  - [X] Phone numbers
- [X] Code-first approach
  - [X] Let EF create the database schema
- [X] Use SQL Server

## Stretch Goals

- [X] Functionality that allows sending e-mail from the app
- [X] Categories of contacts (Family, Friends, Work, etc.)
- [ ] Send SMS

# Features

- Management of contacts
- Categorization of contacts
- Basic search functionality
- Send email to contact
- EF Core based app
  - Pure code design

# Challenges

EF Core took a little bit of work to get my head around. The migrations path especially.  
This was not helped by Rider not finding the `dotnet ef` for some reason (it had the path and command perfect, I just 
had to copy the failed command to my terminal and run it there).  
Twilio has a bunch of regulatory requirements in the UK, so it's taking a while to get a phone number allocated.  
Generally though this has been smooth. I was expecting SMTP to give me more trouble than it did...

# Lessons Learned

- EF Core is pretty interesting
  - Whether I prefer it to something lighter weight like Dapper, I'm not yet sure
- Found lots of new libraries!

# Areas to Improve

- Spectre seemed quite limited for multi-line input.
  - I think I'd like to have a go at a proper text editor some day
- Just need to use EF a lot more

# Resources Used

- Microsoft Docs (lots of it)
- MailKit
- libphonenumber-csharp
- Spectre.Console
- A bit of StackOverflow
