using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class EventsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
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