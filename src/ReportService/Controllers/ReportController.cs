using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using ReportService.Entities;

namespace ReportService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController: ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Report>>> GetAllReports()
    {
        var query = DB.Find<Report>();
        query.Sort(x => x.Ascending(a => a.ReportStatus));
        var result = await query.ExecuteAsync();

        return result;
    }
}