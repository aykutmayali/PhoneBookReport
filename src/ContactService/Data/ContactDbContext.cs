using ContactService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data;

public class ContactDbContext:DbContext
{
    public ContactDbContext(DbContextOptions options): base(options){}

    public DbSet<Contact> Contacts{get; set;}
}