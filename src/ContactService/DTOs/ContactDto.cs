namespace ContactService.DTOs;

public class ContactDto
{
    public Guid Id {get; set; }
    public string Name {get; set;}
    public string Surname {get; set;}
    public string CompanyName {get; set;}
    public string ContactType {get; set;}
}