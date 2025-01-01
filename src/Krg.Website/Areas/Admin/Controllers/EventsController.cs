using Krg.Domain;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Krg.Website.Areas.Admin.Models;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class EventsController(IEventDateService eventDateService, IOptions<SiteSettings> siteSettings) : Controller
{
    private readonly int[] _months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    private readonly SiteSettings _siteSettings = siteSettings.Value;
    
    [HttpGet]
    public IActionResult Index(string? tab = null)
    {
        // var allDates = eventDateService
        //     .GetEvents(DateTime.Now.Year)
        //     .OrderBy(x => x.Date)
        //     .ToList();
        //
        // var groupedDates = GroupEventsByDate(allDates);

        return View(new EventsViewModel
        {
            SelectedDate = DateTime.Now,
            // Year = allDates.FirstOrDefault().Date.Year,
            // Events = groupedDates
        });
    }

    [HttpPost]
    public IActionResult Index(int year)
    {
        // if (year != DateTime.Now.Year && !_siteSettings.YearsToShow.Contains(year))
        // {
        //     year = DateTime.Now.Year;
        // }
        //
        // var allDates = eventDateService
        //     .GetEvents(year)
        //     .OrderBy(x => x.Date)
        //     .ToList();
        //
        // var groupedDates = GroupEventsByDate(allDates);

        return View(new EventsViewModel
        {
            SelectedDate = DateTime.Now,
            // Year = allDates.FirstOrDefault().Date.Year,
            // Events = groupedDates
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateEventDto createEventDto)
    {
        if (DateTime.TryParse(createEventDto.Date, out DateTime dateTimeParsed))
        {
            var eventDate = eventDateService.GetEventByDate(dateTimeParsed);
            
            if(eventDate != null)
                return RedirectToAction(nameof(Index));
            
            eventDateService.AddEventDate(new EventDate
            {
                Date = dateTimeParsed,
                ContactName = createEventDto.ContactName,
                ContactPhone = createEventDto.ContactPhoneNo,
                ContactEmail = createEventDto.ContactEmail
            });
            
            return RedirectToAction(nameof(Index), new {tab = dateTimeParsed.ToDkMonth()});
        }
        return BadRequest("Error parsing date");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EditEventDto editEventDto)
    {
        if (DateTime.TryParse(editEventDto.Date, out DateTime dateTimeParsed))
        {
            var eventDate = eventDateService.GetEventById(editEventDto.Id);

            if (eventDate != null)
            {
                eventDate.ContactName = editEventDto.ContactName;
                eventDate.ContactPhone = editEventDto.ContactPhoneNo;
                eventDate.ContactEmail = editEventDto.ContactEmail;
                
                eventDateService.UpdateEventDate(eventDate);

                return RedirectToAction(nameof(Index), new {tab = dateTimeParsed.ToDkMonth()});
            }
        }

        return BadRequest();
    }
    
    public IActionResult Edit(int id)
    {
        var eventDate = eventDateService.GetEventById(id);

        if (eventDate != null)
        {
            return View(new EditEventDto
            {
                Id = eventDate.Id,
                Date = eventDate.Date.ToString("yyyy-MM-dd"),
                ContactName = eventDate.ContactName,
                ContactPhoneNo = eventDate.ContactPhone,
                ContactEmail = eventDate.ContactEmail
            });
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult Delete(int id)
    {
        eventDateService.RemoveEventDate(id);
            
        return RedirectToAction(nameof(Index));
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