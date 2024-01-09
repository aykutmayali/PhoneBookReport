using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactService.DTOs;
using ContactService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext _context;
    private readonly IMapper _mapper;

    public ContactRepository(ContactDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public void AddContact(Contact contact)
    {
        _context.Contacts.Add(contact);
    }

    public async Task<ContactDto> GetContactByIdAsync(Guid id)
    {
        return await _context.Contacts
            .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x =>x.Id == id);
    }

    public async Task<Contact> GetContactEntityById(Guid id)
    {
       return await _context.Contacts
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == id);

    }

    public async Task<List<ContactDto>> GetContactsAsync()
    {
        var contacts = await _context.Contacts
            .Include(x => x.Company)
            .OrderBy(x => x.Company.CompanyName)
            .ToListAsync();

        return _mapper.Map<List<ContactDto>>(contacts);
    }

    public void RemoveContact(Contact contact)
    {
        _context.Contacts.Remove(contact);
    }

    public void RemoveContact(ContactDto contact)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >0;
    }
}