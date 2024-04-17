using Krg.Database;
using Krg.Database.Models;
using Krg.Domain;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Services
{
    public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IContentService _contentService;

		public EventRegistrationService(
			IRegistrationRepository registrationRepository,
			IContentService contentService)
		{
			_registrationRepository = registrationRepository;
			_contentService = contentService;
		}

		public void AddRegistration(int umbracoNodeId, Registration eventRegistration)
		{
			_registrationRepository.AddRegistration(new EventRegistration
			{
				BringsTrailer = eventRegistration.BringsTrailer,
				Department = eventRegistration.Department,
				Email = eventRegistration.Email,
				EventDate = eventRegistration.EventDate,
				Name = eventRegistration.Name,
				NoOfAdults = eventRegistration.NoOfAdults,
				NoOfChildren = eventRegistration.NoOfChildren,
				PhoneNo = eventRegistration.PhoneNo,
				ShowName = eventRegistration.ShowName,
				UmbracoEventNodeId = umbracoNodeId,
				UpdateTimeUtc = DateTime.UtcNow
			});
		}

		public List<RegistrationDto> GetRegistrations()
		{
			var dbRegistrations = _registrationRepository
				.GetRegistrations()
				.Select(reg => new Registration(reg))
				.ToList();

			var registrationRoot = Constants.RegistrationRootId; //Todo read from config
			
			var umbracoRegistrations = _contentService
				.GetPagedDescendants(registrationRoot, 0, 100, out long totalRecords)
				.Where(x => x.ContentTypeId == Constants.EventTypeId); //Todo select by node alias

			List<RegistrationDto> results = new List<RegistrationDto>();

			foreach (IContent umbRegistration in umbracoRegistrations)
			{
				RegistrationDto registrationDto = new RegistrationDto
				{
					Content = umbRegistration,
					Registrations = dbRegistrations.Where(x => x.EventDate == umbRegistration.GetValue<DateTime>("date")).ToList(),
				};

				results.Add(registrationDto);
			}
			return results;
		}

		//public void RemoveRegistration(Registration eventRegistration)
		//{
		//	using var scope = _scopeProvider.CreateScope();

		//	scope.Database.Delete<EventRegistration>($"DELETE * FROM EventRegistration WHERE Email = {eventRegistration.Email} && EventDate = {eventRegistration.EventDate}");

		//	scope.Complete();
		//}
	}

	public class RegistrationDto
	{
        public List<Registration> Registrations { get; set; }

		public IContent Content { get; set; }
    }

	public class Constants
	{
		public const int RegistrationRootId = 1060;

		public const int EventTypeId = 1058;
	}
}
