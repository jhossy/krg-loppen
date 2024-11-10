using Krg.Domain.Models;
using Krg.Services;
using Krg.Services.Interfaces;
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
			var eventRoots = _umbracoHelper.EventRoot();

			if (eventRoots == null)
			{
				return CurrentTemplate(new HomePageViewModel(CurrentPage, new PublishedValueFallback(_serviceContext, _variationContextAccessor)));
			}
		
			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

            foreach (var root in eventRoots)
			{
                int exportYear = root.ExportYear > 0 ? root.ExportYear : DateTime.Now.Year;

				var eventsForExportYear = root
					.DescendantsOrSelf<Event>()
					.Where(x => x.Date.Date > (DateTime.Now.AddDays(-1)).Date && x.Date.Year == exportYear)
					.OrderBy(x => x.Date.Date)
					.ToList();

				List<Registration> dbRegistrations = _eventRegistrationService.GetNonDeletedRegistrations(exportYear).ToList();

				results.AddRange(BuildListOfRegistrations(dbRegistrations, eventsForExportYear));
			}

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
				var filteredRegistrationsByDate = dbRegistrations
					.Where(x => x.EventDate.Year == umbRegistration.Date.Year && 
								x.EventDate.Month == umbRegistration.Date.Month && 
								x.EventDate.Day == umbRegistration.Date.Day)
					.ToList();

				var registrationDto = new RegistrationViewModel(umbRegistration, filteredRegistrationsByDate);

				results.Add(registrationDto);
			}

			return results;
		}
	}
}
