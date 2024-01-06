using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
var app = builder.Build();

// Configure the services.
//app.Services.AddAuthorization(); // Add this line to register authorization services
// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
