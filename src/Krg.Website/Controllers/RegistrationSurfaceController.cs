using FluentValidation.Results;
using Krg.Database.Models;
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
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Filters;

namespace Krg.Web.Controllers
{
	public class RegistrationSurfaceController : Controller
	{
		private readonly IEventRegistrationService _eventRegistrationService;
		private readonly IEmailNotificationService _notificationService;
		private readonly UmbracoHelper _umbracoHelper;
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
			UmbracoHelper umbracoHelper,
			ILogger<RegistrationSurfaceController> logger) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_eventRegistrationService = eventRegistrationService;
			_notificationService = notificationService;
			_umbracoHelper = umbracoHelper;
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

			string emailSender = _umbracoHelper.GetEmailSenderOrFallback();

			int eventRegistrationId = _eventRegistrationService.AddRegistration(request.UmbracoNodeId, request);
			
			_notificationService.AddNotification(request, emailSender);

			_notificationService.AddReminder(request, emailSender, eventRegistrationId);

			return RedirectToCurrentUmbracoPage();
		}
	}
}
