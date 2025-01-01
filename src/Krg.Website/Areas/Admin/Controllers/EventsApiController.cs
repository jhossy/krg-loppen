using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class EventsApiController(IEventDateService eventDateService, IOptions<SiteSettings> siteSettings) : ControllerBase
{
    private readonly int[] _months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    private readonly SiteSettings _siteSettings = siteSettings.Value;
    
    [HttpGet]
    public IActionResult GetEvents(int year = 0)
    {
        if (year != DateTime.Now.Year && !_siteSettings.YearsToShow.Contains(year))
        {
            year = DateTime.Now.Year;
        }

        var allDates = eventDateService
            .GetEvents(year)
            .OrderBy(x => x.Date)
            .ToList();
        
        var sortedDates = GroupEventsByDate(allDates);
        
        return Ok(sortedDates);
    }
    
    private Dictionary<string, List<EventDate>> GroupEventsByDate(List<EventDate> allDates)
    {
        Dictionary<string, List<EventDate>> groupedDates = new Dictionary<string, List<EventDate>>();

        foreach (int month in _months)
        {
            var eventsInMonth = new List<EventDate>();
            eventsInMonth.AddRange(allDates.Where(x => x.Date.Month == month));

            string monthName = new DateTime(DateTime.Now.Year, month, 1).ToDkMonth();
            groupedDates.Add(monthName, eventsInMonth);
        }

        return groupedDates;
    }
}