using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;

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
			if (addRegistrationRequest == null) return;

			if (addRegistrationRequest.Department == null || addRegistrationRequest.PhoneNo == null) return;

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

		public List<Registration> GetAllRegistrations()
		{
			return _registrationRepository
				.GetAllRegistrations()
				.Select(reg => new Registration(reg))
				.ToList();
		}

		public List<Registration> GetNonDeletedRegistrations()
		{
			return _registrationRepository
				.GetNonDeletedRegistrations()
				.Select(reg => new Registration(reg))
				.ToList();
		}

		public void RemoveRegistration(int eventId)
		{
			_registrationRepository.RemoveRegistration(eventId);
		}
	}
}
