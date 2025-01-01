using Krg.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Krg.Website.Areas.Admin.Models;

public class EventsViewModel
{
    public DateTime SelectedDate { get; set; } = DateTime.Now;
    // public int Year { get; set; }
    // public Dictionary<string, List<EventDate>>? Events { get; set; }
}