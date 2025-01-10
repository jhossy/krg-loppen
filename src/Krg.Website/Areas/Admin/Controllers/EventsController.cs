using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Krg.Website.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class EventsController(IEventDateService eventDateService, ILogger<EventsController> logger) : BaseAdminController
{
    [HttpGet]
    public IActionResult Index(string tab = default(string))
    {
        return View(new EventsViewModel
        {
            SelectedDate = DateTime.Now,
            Tab = tab
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
            
            logger.LogInformation("Adding new event: {@createEventDto}", createEventDto);
            
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
            
            logger.LogInformation("Updating event - old values: {@eventDate}", eventDate);
            
            if (eventDate != null)
            {
                eventDate.ContactName = editEventDto.ContactName;
                eventDate.ContactPhone = editEventDto.ContactPhoneNo;
                eventDate.ContactEmail = editEventDto.ContactEmail;
                
                eventDateService.UpdateEventDate(eventDate);
                
                logger.LogInformation("Updating event - new values: {@editEventDto}", editEventDto);

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
        logger.LogInformation($"Deleting event with id: {id}");
        
        eventDateService.RemoveEventDate(id);
            
        return RedirectToAction(nameof(Index));
    }
}