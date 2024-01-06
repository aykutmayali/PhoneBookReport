using System.ComponentModel.DataAnnotations.Schema;

namespace ContactService.Entities;

[Table("Companies")]
public class Company
{
    public Guid Id {get; set; }
    public string CompanyName {get; set;}
    public ContactType ContactType {get; set;}
    public string DataContent {get; set;}

    //nav properties
    public Contact Contact {get; set;}
    public Guid ContactId {get; set;}
}