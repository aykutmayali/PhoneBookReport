using Contracts;
using MassTransit;

namespace ContactService.Consumers;

public class ContactCreatedFaultConsumer : IConsumer<Fault<SourceCreated>>
{
    public async Task Consume(ConsumeContext<Fault<SourceCreated>> context)
    {
        System.Console.WriteLine("---> Consuming Faulty Creation");
        var exception = context.Message.Exceptions.First();
        if(exception.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.ReportDetail = "";
            await context.Publish(context.Message.Message);
        } else 
        {
            System.Console.WriteLine("Not an argument exception - update dashboard error");
        }
    }
}