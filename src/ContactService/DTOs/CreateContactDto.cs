using System.ComponentModel.DataAnnotations;

namespace ContactService.DTOs;

public class CreateContactDto
{
    [Required]
    public string Name {get; set;}
    [Required]
    public string Surname {get; set;}
    [Required]
    public string CompanyName {get; set;}
    [Required]
    public string ContactType {get; set;}
}