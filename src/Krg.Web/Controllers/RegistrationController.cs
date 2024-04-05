using Krg.Domain;
using Krg.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;

namespace Krg.Web.Controllers
{
	public class RegistrationController : Umbraco.Cms.Web.Website.Controllers.SurfaceController
	{
		private readonly IEventRegistrationService _eventRegistrationService;

		public RegistrationController(
			IUmbracoContextAccessor umbracoContextAccessor, 
			IUmbracoDatabaseFactory databaseFactory, 
			ServiceContext services, 
			AppCaches appCaches, 
			IProfilingLogger profilingLogger, 
			IPublishedUrlProvider publishedUrlProvider,
			IEventRegistrationService eventRegistrationService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_eventRegistrationService = eventRegistrationService;
		}

		[ValidateUmbracoFormRouteString]
		[HttpPost]
		public async Task<IActionResult> HandleSubmit(AddRegistrationRequest request)
		{
			await _eventRegistrationService.AddRegistration(
				new Registration
				{ 
					Name = "test testesen",
					Email = "test@email.dk",
					EventDate = request.EventDate,
					NoOfAdults = 3,
					NoOfChildren = 3,
					PhoneNo = "1234567890",
					Department = "Mikro",
					BringsTrailer = true,
					ShowName = true,
				}
			);

            return RedirectToCurrentUmbracoPage();
		}
	}
}
