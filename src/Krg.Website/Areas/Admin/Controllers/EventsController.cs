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
}