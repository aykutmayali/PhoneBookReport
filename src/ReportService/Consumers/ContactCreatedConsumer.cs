using MassTransit;
using Contracts;
using AutoMapper;
using ReportService.Entities;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Entities;

namespace ReportService.Consumers;

public class ContactCreatedConsumer : IConsumer<SourceCreated>
{
    private readonly IMapper _mapper;
    public ContactCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<SourceCreated> context)
    {
        System.Console.WriteLine("---> Consuming report created: " + context.Message.Id);

        var report = _mapper.Map<Report>(context.Message);

        await report.SaveAsync();
    }
}