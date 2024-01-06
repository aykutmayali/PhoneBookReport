using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using ReportService.Entities;
using ReportService.RequestHelpers;
using ZstdSharp.Unsafe;

namespace ReportService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController: ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Report>>> GetReports([FromQuery] SearchParams searchParams, string filterBy)
    {
        var query = DB.PagedSearch<Report, Report>();
        if(!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        if (!string.IsNullOrEmpty(filterBy))
        {
            query = query.Match(r => r.ReportDetail.Contains(filterBy));
        }

        query = searchParams.OrderBy switch
        {
            "requestDate" => query.Sort(x => x.Descending(a => a.RequestDate))
                .Sort(x => x.Ascending(a =>a.ReportStatus)),
            "reportStatus" => query.Sort(x => x.Ascending(a => a.ReportStatus)),
            _ => query.Sort(x => x.Ascending(a => a.RequestDate))
        };

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}










//     [HttpGet]
//     public async Task<ActionResult<List<Report>>> GetAllReports()
//     {
//         var query = DB.Find<Report>();
//         query.Sort(x => x.Ascending(a => a.ReportStatus));
//         var result = await query.ExecuteAsync();

//         return result;
//     }