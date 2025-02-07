namespace Krg.Website.Areas.Admin.Models;

public class EventsViewModel
{
    public DateTime SelectedDate { get; init; } = DateTime.Now;

    public string Tab { get; set; } = string.Empty;
}