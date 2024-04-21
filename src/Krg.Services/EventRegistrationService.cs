using Krg.Database;
using Krg.Database.Models;
using Krg.Domain;

namespace Krg.Services
{
	public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IRegistrationRepository _registrationRepository;

		public EventRegistrationService(IRegistrationRepository registrationRepository)
		{
			_registrationRepository = registrationRepository;
		}

		public void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest)
		{
			//todo add validation
			_registrationRepository.AddRegistration(new EventRegistration
			{
				BringsTrailer = addRegistrationRequest.BringsTrailer,
				Department = addRegistrationRequest.Department,
				Email = addRegistrationRequest.Email,
				EventDate = addRegistrationRequest.EventDate,
				Name = addRegistrationRequest.Name,
				NoOfAdults = addRegistrationRequest.NoOfAdults,
				NoOfChildren = addRegistrationRequest.NoOfChildren,
				PhoneNo = addRegistrationRequest.PhoneNo,
				ShowName = addRegistrationRequest.ShowName,
				UmbracoEventNodeId = umbracoNodeId,
				UpdateTimeUtc = DateTime.UtcNow
			});
		}

		public List<Registration> GetRegistrations()
		{
			return _registrationRepository
				.GetRegistrations()
				.Select(reg => new Registration(reg))
				.ToList();
		}
	}
}
