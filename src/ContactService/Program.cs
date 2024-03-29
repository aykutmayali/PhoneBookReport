using ContactService.Consumers;
using ContactService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ContactDbContext>(opt =>{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => 
    {
        x.AddEntityFrameworkOutbox<ContactDbContext>(o => {
            o.QueryDelay = TimeSpan.FromSeconds(10);

            o.UsePostgres();

            o.UseBusOutbox();
        });

        x.AddConsumersFromNamespaceContaining<ContactCreatedFaultConsumer>();
        x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("report", false));

        x.UsingRabbitMq((context, cfg) => 
        {
            cfg.ConfigureEndpoints(context);
        });
    });

builder.Services.AddScoped<IContactRepository, ContactRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
