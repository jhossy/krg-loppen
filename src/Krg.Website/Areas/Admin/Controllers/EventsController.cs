using Krg.Services.Interfaces;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class EventsController(IEventDateService eventDateService) : Controller
{
    private readonly IEventDateService _eventDateService = eventDateService;

    // GET
    public IActionResult Index()
    {
        return View(new EventsViewModel {Events = _eventDateService.GetEvents(DateTime.Now.Year)});
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string startDate)
    {
        if (DateTime.TryParse(startDate, out DateTime dateTimeParsed))
        {
            return Ok($"Success - {startDate}");
        }
        return BadRequest("Error parsing date");
    }
}