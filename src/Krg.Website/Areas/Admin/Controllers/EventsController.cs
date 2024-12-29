using Krg.Domain;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class EventsController(IEventDateService eventDateService) : Controller
{
    private readonly IEventDateService _eventDateService = eventDateService;

    // GET
    public IActionResult Index(string edit = null)
    {
        var allDates = _eventDateService.GetEvents(DateTime.Now.Year).OrderBy(x => x.Date);

        Dictionary<string, List<EventDate>> groupedDates = new Dictionary<string, List<EventDate>>();
        int[] months = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        
        foreach (int month in months)
        {
            var eventsInMonth = new List<EventDate>();
            eventsInMonth.AddRange(allDates.Where(x => x.Date.Month == month));

            string monthName = new DateTime(DateTime.Now.Year, month, 1).ToDkMonth();
            groupedDates.Add(monthName, eventsInMonth);
        }

        DateTime selectedDate;

        if (string.IsNullOrEmpty(edit) || !DateTime.TryParse(edit, out selectedDate))
        {
            selectedDate = DateTime.Now;
        }
        
        return View(new EventsViewModel
        {
            SelectedDate = selectedDate,
            Year = allDates.FirstOrDefault().Date.Year,
            Events = groupedDates
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateEventDto createEventDto)
    {
        if (DateTime.TryParse(createEventDto.Date, out DateTime dateTimeParsed))
        {
            var eventDate = _eventDateService.GetEventByDate(dateTimeParsed);
            
            if(eventDate != null)
                return RedirectToAction(nameof(Index));
            
            _eventDateService.AddEventDate(new EventDate
            {
                Date = dateTimeParsed,
                ContactName = createEventDto.ContactName,
                ContactPhone = createEventDto.ContactPhoneNo,
                ContactEmail = createEventDto.ContactEmail
            });
            
            return RedirectToAction(nameof(Index));
        }
        return BadRequest("Error parsing date");
    }
    
    public IActionResult Edit(string date)
    {
        if (DateTime.TryParse(date, out DateTime dateTimeParsed))
        {
            var eventDate = _eventDateService.GetEventByDate(dateTimeParsed);

            if (eventDate != null)
            {
                return View(new EditEventDto
                {
                    Date = eventDate.Date.ToString("yyyy-MM-dd"),
                    ContactName = eventDate.ContactName,
                    ContactPhoneNo = eventDate.ContactPhone,
                    ContactEmail = eventDate.ContactEmail
                });
            }
        }

        return BadRequest();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EditEventDto editEventDto)
    {
        if (DateTime.TryParse(editEventDto.Date, out DateTime dateTimeParsed))
        {
            var eventDate = _eventDateService.GetEventByDate(dateTimeParsed);

            if (eventDate != null)
            {
                eventDate.ContactName = editEventDto.ContactName;
                eventDate.ContactPhone = editEventDto.ContactPhoneNo;
                eventDate.ContactEmail = editEventDto.ContactEmail;
                
                _eventDateService.UpdateEventDate(eventDate);

                return RedirectToAction(nameof(Index));
            }
        }

        return BadRequest();
    }
    
    public IActionResult Delete(string date)
    {
        if (DateTime.TryParse(date, out DateTime dateTimeParsed))
        {
            var eventDate = _eventDateService.GetEventByDate(dateTimeParsed);
            
            if(eventDate == null)
                return RedirectToAction(nameof(Index));

            _eventDateService.RemoveEventDate(eventDate);
            
            return RedirectToAction(nameof(Index));
        }
        return BadRequest("Error parsing date");
    }
}

public class CreateEventDto
{
    public string Date { get; set; }

    public string ContactName { get; set; }

    public string ContactPhoneNo { get; set; }

    public string ContactEmail { get; set; }
}

public class EditEventDto
{
    public string Date { get; set; }

    public string ContactName { get; set; }

    public string ContactPhoneNo { get; set; }

    public string ContactEmail { get; set; }
}