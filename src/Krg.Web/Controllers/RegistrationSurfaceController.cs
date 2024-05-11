using FluentValidation.Results;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Krg.Web.Models;
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
		private readonly IEmailNotificationService _notificationService;
		private readonly ILogger<RegistrationSurfaceController> _logger;

		public RegistrationSurfaceController(
			IUmbracoContextAccessor umbracoContextAccessor, 
			IUmbracoDatabaseFactory databaseFactory, 
			ServiceContext services, 
			AppCaches appCaches, 
			IProfilingLogger profilingLogger, 
			IPublishedUrlProvider publishedUrlProvider,
			IEventRegistrationService eventRegistrationService,
			IEmailNotificationService notificationService,
			ILogger<RegistrationSurfaceController> logger) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_eventRegistrationService = eventRegistrationService;
			_notificationService = notificationService;
			_logger = logger;
		}

		[ValidateUmbracoFormRouteString]
		[HttpPost]
		public IActionResult HandleSubmit(AddRegistrationRequest request)
		{
			AddRegistrationRequestValidator validator = new AddRegistrationRequestValidator();
			ValidationResult validationResult = validator.Validate(request);

			if (!ModelState.IsValid || !validationResult.IsValid)
			{
				_logger.LogError($"Error when trying to sign up for event. NodeId: {request.UmbracoNodeId}, EventDate: {request.EventDate} - {validationResult}");

				return RedirectToCurrentUmbracoPage(); //CurrentUmbracoPage() keeps 'old' viewstate
			}

			_eventRegistrationService.AddRegistration(request.UmbracoNodeId, request);

			_notificationService.AddNotification(request);

			return RedirectToCurrentUmbracoPage();
		}
	}
}
