using MongoDB.Entities;

namespace ReportService.Entities;

public class Report: Entity
{
    //public Guid Id {get; set; }
    public DateTime RequestDate {get; set;}
    public ReportStatus ReportStatus {get; set;}
    public string ReportDetail {get; set;}
}