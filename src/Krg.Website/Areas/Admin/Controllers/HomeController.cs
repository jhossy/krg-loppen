using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers
{
	public class HomeController : BaseAdminController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
