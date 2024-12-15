using FluentValidation.Results;
using Krg.Domain.Models;
using Krg.Services;
using Krg.Services.Interfaces;
using Krg.Web.Models;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Krg.Web.Controllers
{
    public class HomeController : Controller
	{
		private readonly IEventRegistrationService _eventRegistrationService;		
		private readonly IEmailNotificationService _notificationService;
		private readonly IEventDateService _eventDateService;
		private readonly SiteSettings _siteSettings;

		private readonly ILogger _logger;

		public HomeController(
			ILogger<HomeController> logger,
			IEventRegistrationService eventRegistrationService,
			IEmailNotificationService notificationService,
			IEventDateService eventDateService,
			IOptions<SiteSettings> siteSettings)
		{
			_eventRegistrationService = eventRegistrationService;
			_notificationService = notificationService;
			_eventDateService = eventDateService;
			_siteSettings = siteSettings.Value;
			_logger = logger;
		}

		public IActionResult Index()
		{
			int[] yearsToShow = _siteSettings.YearsToShow;

			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

			foreach (var year in yearsToShow)
			{
				List<EventDate> events = _eventDateService.GetEvents(year);

				List<Registration> dbRegistrations = _eventRegistrationService.GetNonDeletedRegistrations(year).ToList();

				results.AddRange(BuildListOfRegistrations(dbRegistrations, events));
			}

			var viewModel = new HomeViewModel
			{
				Events = results.OrderBy(evt => evt.EventContent.Date).ToList()
			};

			return View(viewModel);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult HandleSubmit(AddRegistrationRequest request)
		{
			AddRegistrationRequestValidator validator = new AddRegistrationRequestValidator();

			ValidationResult validationResult = validator.Validate(request);

			if (!ModelState.IsValid || !validationResult.IsValid)
			{
				_logger.LogError($"Error when trying to sign up for event. EventDate: {request.EventDate} - {validationResult}");

				return View("Index");
			}

			string emailSender = _siteSettings.EmailFromAddress;

			int eventRegistrationId = _eventRegistrationService.AddRegistration(request);

			_notificationService.AddNotification(request, emailSender);

			_notificationService.AddReminder(request, emailSender, eventRegistrationId);

			return RedirectToAction("Index");
		}

		internal List<RegistrationViewModel> BuildListOfRegistrations(List<Registration> dbRegistrations, List<EventDate> eventDates)
		{
			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

			foreach (var eventDate in eventDates)
			{
				var filteredRegistrationsByDate = dbRegistrations
					.Where(x => x.EventDate.Year == eventDate.Date.Year &&
								x.EventDate.Month == eventDate.Date.Month &&
								x.EventDate.Day == eventDate.Date.Day)
					.ToList();

				var registrationDto = new RegistrationViewModel(eventDate, filteredRegistrationsByDate);

				results.Add(registrationDto);
			}

			return results;
		}
	}
}
