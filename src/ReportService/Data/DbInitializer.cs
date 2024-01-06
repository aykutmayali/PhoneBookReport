using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using ReportService.Entities;

namespace ReportService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("ReportDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Report>()
            .Key(x =>x.RequestDate, KeyType.Text)
            .Key(x =>x.ReportStatus, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Report>();

        if(count == 0){
            System.Console.WriteLine("No data - will attemt to seed");
            var reportData = await File.ReadAllTextAsync("Data/contacts.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var reports = JsonSerializer.Deserialize<List<Report>>(reportData, options);

            await DB.SaveAsync(reports);
        }
    }
}