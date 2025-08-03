using System.Net;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Krg.Website.Areas.Admin.Models;
using Krg.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class EventsApiController(IEventDateService eventDateService, IOptions<SiteSettings> siteSettings, ILogger<EventsApiController> logger) : ControllerBase
{
    private readonly int[] _months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    private readonly SiteSettings _siteSettings = siteSettings.Value;
   
    [HttpPost]
    public ActionResult CreateEvent([FromBody] CreateEventDto createEventDto)
    {
        if (DateTime.TryParse(createEventDto.Date, out DateTime dateTimeParsed))
        {
            var eventDate = eventDateService.GetEventByDate(dateTimeParsed);

            if (eventDate != null)
            {
                return Ok(GetGroupedEventsByDate(dateTimeParsed.Year));
            }

            logger.LogInformation("Adding new event: {@createEventDto}", createEventDto);
            
            eventDateService.AddEventDate(new EventDate
            {
                Date = dateTimeParsed,
                ContactName = createEventDto.ContactName,
                ContactPhone = createEventDto.ContactPhoneNo,
                ContactEmail = createEventDto.ContactEmail,
                Note = WebUtility.HtmlEncode(createEventDto.Note)
            });
            
            return Ok(GetGroupedEventsByDate(dateTimeParsed.Year));
        }
        
        logger.LogError($"Error attempting to create event: {createEventDto}");
        
        return BadRequest("Error parsing date");
    }
    
    [HttpGet]
    public IActionResult GetEvents(int year = 0)
    {
        return Ok(GetGroupedEventsByDate(year));
    }

    private Dictionary<string, List<EventDate>> GetGroupedEventsByDate(int year = 0)
    {
        if (year != DateTime.Now.Year && !_siteSettings.YearsToShow.Contains(year))
        {
            year = DateTime.Now.Year;
        }
        
        var allDates = eventDateService
            .GetEvents(year)
            .OrderBy(x => x.Date)
            .ToList();
        
        return GroupEventsByDate(allDates);
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