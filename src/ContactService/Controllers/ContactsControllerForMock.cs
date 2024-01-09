using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using AutoMapper;
using ContactService.Data;
using ContactService.DTOs;
using ContactService.Entities;
using Contracts;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Controllers;
[ApiController]
[Route("api/contacts")]
public class ContactsControllerForMock: ControllerBase
{
    private readonly IContactRepository _repo;
    private readonly IMapper _mapper;
    private readonly Random _random;
    private readonly IPublishEndpoint _publishedEndPoint;
    public ContactsControllerForMock(IContactRepository repo, IMapper mapper, IPublishEndpoint publishedEndPoint)
    {
        _repo = repo; 
        _mapper = mapper;
        _publishedEndPoint = publishedEndPoint;
        _random = new Random();
    }

    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetAllContacts()
    {
        return await _repo.GetContactsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContactById(Guid id)
    {
        var contact = await _repo.GetContactByIdAsync(id);

        if(contact == null) return NotFound();

        return contact;
    }

    [HttpGet("source")]
    public ActionResult<List<ContactDto>> GetSourceContacts()
    {
        var contacts = _repo.GetContactsAsync();
        var sourceDtos = contacts.Select(
            contact => {
                var sourceDto = new SourceDto
                {
                    RequestDate = DateTime.UtcNow,
                    ReportStatus = _random.Next(1,3),
                    ReportDetail = $"{contact}"
                };
                var newReport = _mapper.Map<SourceDto>(sourceDto);
                _publishedEndPoint.Publish(_mapper.Map<SourceCreated>(newReport));
                return sourceDto;
            });
        return Ok(sourceDtos);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto contactDto)
    {
        var contact = _mapper.Map<Contact>(contactDto);

        _repo.AddContact(contact);

        var result = await _repo.SaveChangesAsync();

        if(!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetContactById), new {contact.Id}, _mapper.Map<ContactDto>(contact));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContact(Guid id, UpdateContactDto updateContactDto){
        var contact = await _repo.GetContactByIdAsync(id);

        if(contact == null) return NotFound();

        contact.Name = updateContactDto.Name ?? contact.Name;
        contact.Surname = updateContactDto.Surname ?? contact.Surname;
        
        contact.CompanyName = updateContactDto.CompanyName ?? contact.CompanyName;
        contact.DataContent = updateContactDto.DataContent ?? contact.DataContent;
        // Check if updateContactDto.ContactType is not null before assigning
        contact.ContactType = updateContactDto.ContactType != null
            ? Enum.Parse<ContactType>(updateContactDto.ContactType).ToString()
            : contact.ContactType;

        var result = await _repo.SaveChangesAsync();

        if(result) return Ok();

        return BadRequest("Problem at saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(Guid id)
    {
        var contact = await _repo.GetContactByIdAsync(id);

        if(contact == null) return NotFound();

        _repo.RemoveContact(contact);


        var result = await _repo.SaveChangesAsync();

        if(!result) return BadRequest("Could not update DB");

        return Ok();
    }
}