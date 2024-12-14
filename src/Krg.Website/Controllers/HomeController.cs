using FluentValidation.Results;
using Krg.Database.Interfaces;
using Krg.Domain.Models;
using Krg.Services;
using Krg.Services.Interfaces;
using Krg.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Web.Controllers
{
    public class HomeController : Controller
	{
		private readonly IEventRegistrationService _eventRegistrationService;		
		private readonly IEmailNotificationService _notificationService;
		private readonly IUnitOfWork _unitOfWork;

		private readonly ILogger _logger;

		public HomeController(
			ILogger<HomeController> logger,
			IEventRegistrationService eventRegistrationService,
			IEmailNotificationService notificationService,
			IUnitOfWork unitOfWork)
		{
			_eventRegistrationService = eventRegistrationService;
			_notificationService = notificationService;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public IActionResult Index()
		{
			List<int> yearsToShow = new List<int> { 2024, 2025 }; //TODO from settings

			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

			foreach (var year in yearsToShow)
			{
				List<EventDate> events = _unitOfWork.EventRepository
					.GetAllEvents(year)
					.Select(x => 
						new EventDate 
						{
							Date = x.Date,							
							ContactEmail = x.ContactEmail,
							ContactName = x.ContactName,
							ContactPhone = x.ContactPhone,
						})
					.ToList(); //TODO move to event service

				List<Registration> dbRegistrations = _eventRegistrationService.GetNonDeletedRegistrations(year).ToList();

				results.AddRange(BuildListOfRegistrations(dbRegistrations, events));
			}

			var viewModel = new HomeViewModel
			{
				Events = results
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
				_logger.LogError($"Error when trying to sign up for event. NodeId: {request.UmbracoNodeId}, EventDate: {request.EventDate} - {validationResult}");

				return View("Index");
			}

			string emailSender = "from-address"; /*_umbracoHelper.GetEmailSenderOrFallback();*/ //todo settings

			int eventRegistrationId = _eventRegistrationService.AddRegistration(request.UmbracoNodeId, request);

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
