using AutoMapper;
using ContactService.DTOs;
using ContactService.Entities;
using Contracts;

namespace ContactService.RequestHelpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Contact, ContactDto>().IncludeMembers(x => x.Company);
        CreateMap<Company, ContactDto>();
        CreateMap<CreateContactDto, Contact>().ForMember(d=> d.Company, o => o.MapFrom(s => s));
        CreateMap<CreateContactDto, Company>();
        CreateMap<SourceDto, SourceCreated>();
    }
}