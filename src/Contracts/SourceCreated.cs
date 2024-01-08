namespace Contracts;

public class SourceCreated
{
    public Guid Id {get; set; }
    public DateTime RequestDate {get; set;}
    public int ReportStatus {get;set;}
    public string ReportDetail{get; set;}
}