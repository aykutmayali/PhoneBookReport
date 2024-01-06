using AutoMapper;
using ContactService.Data;
using ContactService.DTOs;
using ContactService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Controllers;
[ApiController]
[Route("api/contacts")]
public class ContactsController: ControllerBase
{
    private readonly ContactDbContext _context;
    private readonly IMapper _mapper;
    public ContactsController(ContactDbContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetAllContacts()
    {
        var contacts = await _context.Contacts.Include(x => x.Company).OrderBy(x => x.Company.CompanyName).ToListAsync();

        return _mapper.Map<List<ContactDto>>(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContactById(Guid id)
    {
        var contact = await _context.Contacts.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);

        if(contact == null) return NotFound();

        return _mapper.Map<ContactDto>(contact);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto contactDto)
    {
        var contact = _mapper.Map<Contact>(contactDto);

        _context.Contacts.Add(contact);

        var result = await _context.SaveChangesAsync()>0;

        if(!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetContactById), new {contact.Id}, _mapper.Map<ContactDto>(contact));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContact(Guid id, UpdateContactDto updateContactDto){
        var contact = await _context.Contacts.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);

        if(contact == null) return NotFound();

        contact.Name = updateContactDto.Name ?? contact.Name;
        contact.Surname = updateContactDto.Surname ?? contact.Surname;
        contact.Company.CompanyName = updateContactDto.CompanyName ?? contact.Company.CompanyName;

        // Check if updateContactDto.ContactType is not null before assigning
        contact.Company.ContactType = updateContactDto.ContactType != null
            ? Enum.Parse<ContactType>(updateContactDto.ContactType)
            : contact.Company.ContactType;

        var result = await _context.SaveChangesAsync() >0;

        if(result) return Ok();

        return BadRequest("Problem at saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if(contact == null) return NotFound();

        _context.Contacts.Remove(contact);

        var result = await _context.SaveChangesAsync() >0;

        if(!result) return BadRequest("Could not update DB");

        return Ok();
    }
}