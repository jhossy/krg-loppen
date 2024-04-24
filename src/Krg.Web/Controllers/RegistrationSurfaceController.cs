using Krg.Domain;
using Krg.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;

namespace Krg.Web.Controllers
{
	public class RegistrationSurfaceController : Umbraco.Cms.Web.Website.Controllers.SurfaceController
	{
		private readonly IEventRegistrationService _eventRegistrationService;

		public RegistrationSurfaceController(
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
		public IActionResult HandleSubmit(AddRegistrationRequest request)
		{
			if (!ModelState.IsValid) return RedirectToCurrentUmbracoPage(); //CurrentUmbracoPage() keeps 'old' viewstate

			//todo add fluent validation
			_eventRegistrationService.AddRegistration(request.UmbracoNodeId, request);

            return RedirectToCurrentUmbracoPage();
		}
	}
}
