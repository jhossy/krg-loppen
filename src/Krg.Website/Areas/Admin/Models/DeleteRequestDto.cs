namespace Krg.Website.Areas.Admin.Models;

public class DeleteRequestDto
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}