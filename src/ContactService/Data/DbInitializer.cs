
using ContactService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<ContactDbContext>());
    }

    private static void SeedData(ContactDbContext context)
    {
        context.Database.Migrate();
        if (context.Contacts.Any())
        {
            Console.WriteLine("Already have data- no need to seed");
            return;
        }

        var contacts = new List<Contact>
    {
        new Contact { Id = Guid.NewGuid(), Name = "John", Surname = "Doe", 
        Company = new Company{
            Id = Guid.NewGuid(), CompanyName = "Company John", ContactType = ContactType.MobileNumber, DataContent ="555 699 9090"  
        } },

        new Contact { Id = Guid.NewGuid(), Name = "Jane", Surname = "Smith", 
        Company = new Company{
            Id = Guid.NewGuid(), CompanyName = "Company Jane", ContactType = ContactType.EmailAddress, DataContent ="jane@work.com"  
        } }
    };
        //context.Contacts.AddRange(contacts);
        context.AddRange(contacts);
        context.SaveChanges();
    }
}