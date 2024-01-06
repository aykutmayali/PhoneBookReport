namespace ContactService.Entities;

public class Contact 
{
    public Guid Id {get; set; }
    public string Name {get; set;}
    public string Surname {get; set;}
    public Company Company {get; set;}
    
}