namespace Krg.Website.Areas.Admin.Models;

public class EditEventDto
{
    public int Id { get; set; }
    
    public required string Date { get; set; }

    public required string ContactName { get; set; }

    public required string ContactPhoneNo { get; set; }

    public required string ContactEmail { get; set; }
}