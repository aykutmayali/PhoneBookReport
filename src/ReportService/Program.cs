using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using ReportService.Data;
using ReportService.Entities;
using ReportService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<ContactServiceHttpClient>().AddPolicyHandler(GetPolicy());
// Add services to the container.
var app = builder.Build();

// Configure the services.
//app.Services.AddAuthorization(); // register authorization services
// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async ()=> 
{
    await Policy.Handle<TimeoutException>()
        .WaitAndRetryAsync(5, retryAttempt=> TimeSpan.FromSeconds(10))
        .ExecuteAndCaptureAsync(async ()=> await DbInitializer.InitDb(app));

});

// try
// {
//     await DbInitializer.InitDb(app);
// }
// catch (Exception e)
// {
//     System.Console.WriteLine(e);
// }

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));