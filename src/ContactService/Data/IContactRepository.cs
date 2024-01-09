using ContactService.DTOs;
using ContactService.Entities;

namespace ContactService.Data;

public interface IContactRepository
{
    Task<List<ContactDto>>GetContactsAsync();
    Task<ContactDto>GetContactByIdAsync(Guid id);
    Task<Contact>GetContactEntityById(Guid id);
    void AddContact(Contact contact);
    void RemoveContact(Contact contact);
    Task<bool> SaveChangesAsync();
    void RemoveContact(ContactDto contact);
}