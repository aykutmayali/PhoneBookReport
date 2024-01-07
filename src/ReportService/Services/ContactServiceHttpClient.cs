using MongoDB.Entities;
using ReportService.Entities;

namespace ReportService.Services;

public class ContactServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ContactServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Report>> GetItemsForReportDb()
    {
        return await _httpClient.GetFromJsonAsync<List<Report>>(_config["ContactServiceUrl"]+"/api/contacts/source");
    }

}