using Krg.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Krg.Website.Areas.Admin.Models;

public class EventsViewModel
{
    public DateTime SelectedDate { get; set; } = DateTime.Now;
    public int Year { get; set; }
    public Dictionary<string, List<EventDate>>? Events { get; set; }

    public List<SelectListItem> YearSelector { get; set; } =
    [
        new() { Value = "2024", Text = "2024", Selected = DateTime.Now.Year == 2024}, 
        new() { Value = "2025", Text = "2025", Selected = DateTime.Now.Year == 2025}
    ];
}