using MongoDB.Driver;
using MongoDB.Entities;
using ReportService.Data;
using ReportService.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
var app = builder.Build();

// Configure the services.
//app.Services.AddAuthorization(); // register authorization services
// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    System.Console.WriteLine(e);
}

app.Run();
