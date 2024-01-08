using ContactService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data;

public class ContactDbContext: DbContext
{
    public ContactDbContext(DbContextOptions options): base(options){}

    public DbSet<Contact> Contacts{get; set;}

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}