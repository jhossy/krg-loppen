using Krg.Domain.Models;
using Krg.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace Krg.Web.Controllers
{
    public class HomePageController : RenderController
	{		
		private readonly IVariationContextAccessor _variationContextAccessor;
		private readonly ServiceContext _serviceContext;
		private readonly UmbracoHelper _umbracoHelper;
		private readonly IEventRegistrationService _eventRegistrationService;

		public HomePageController(
			ILogger<HomePageController> logger, 
			ICompositeViewEngine compositeViewEngine,
			IUmbracoContextAccessor umbracoContextAccessor,
			IVariationContextAccessor variationContextAccessor,
			ServiceContext context,
			UmbracoHelper umbracoHelper,
			IEventRegistrationService eventRegistrationService) 
			: base(logger, compositeViewEngine, umbracoContextAccessor)
		{
			_variationContextAccessor = variationContextAccessor;
			_serviceContext = context;
			_umbracoHelper = umbracoHelper;
			_eventRegistrationService = eventRegistrationService;
		}

		public IActionResult HomePage()
		{
			var eventRoot = _umbracoHelper.EventRoot();

			if (eventRoot == null)
			{
				return CurrentTemplate(new HomePageViewModel(CurrentPage, new PublishedValueFallback(_serviceContext, _variationContextAccessor)));
			}

			List<Event> umbEvents = eventRoot.DescendantsOrSelf<Event>().ToList();

			List<Registration> dbRegistrations = _eventRegistrationService.GetNonDeletedRegistrations().ToList();

			List<RegistrationViewModel> results = BuildListOfRegistrations(dbRegistrations, umbEvents);

			var viewModel = new HomePageViewModel(CurrentPage, new PublishedValueFallback(_serviceContext, _variationContextAccessor))
			{
				Events = results
			};

			return CurrentTemplate(viewModel);
		}

		internal List<RegistrationViewModel> BuildListOfRegistrations(List<Registration> dbRegistrations, IEnumerable<Event> umbEvents)
		{
			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

			foreach (var umbRegistration in umbEvents)
			{
				var filteredRegistrationsByDate = dbRegistrations.Where(x => x.EventDate == umbRegistration.Date)
					.ToList();

				var registrationDto = new RegistrationViewModel(umbRegistration, filteredRegistrationsByDate);

				results.Add(registrationDto);
			}

			return results;
		}
	}
}
