using Krg.Domain;
using Krg.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;

namespace Krg.Web.Controllers
{
	public class RegistrationsController : UmbracoAuthorizedJsonController
	{
		private readonly IEventRegistrationService _eventRegistrationService;
        public RegistrationsController(IEventRegistrationService eventRegistrationService)
        {
            _eventRegistrationService = eventRegistrationService;
        }

        public IEnumerable<BackofficeRegistrationDto> GetRegistrations(int year)
		{
			int parsedYear = year == 0 ? DateTime.Now.Year : year;

			return _eventRegistrationService
				.GetRegistrations()
				.Where(x => x.EventDate.Year == parsedYear)
				.OrderBy(reg => reg.EventDate)
				.Select(reg => new BackofficeRegistrationDto(reg))
				.ToList();
		}
	}
}
