using Krg.Domain.Models;

namespace Krg.Website.Models;

public class EventsViewModel
{
    public DateTime SelectedDate { get; set; } = DateTime.Now;
    public int Year { get; set; }
    public Dictionary<string, List<EventDate>>? Events { get; set; }
}