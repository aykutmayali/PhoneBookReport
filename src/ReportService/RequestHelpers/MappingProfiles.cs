using AutoMapper;
using Contracts;
using ReportService.Entities;

namespace ReportService.RequestHelpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<SourceCreated, Report>();
    }
}