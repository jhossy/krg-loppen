namespace Krg.Website.Areas.Admin.Models;

public class CreateEventDto
{
    public required string Date { get; set; }

    public required string ContactName { get; set; }

    public required string ContactPhoneNo { get; set; }

    public required string ContactEmail { get; set; }
    
    public string? Note { get; set; }
}