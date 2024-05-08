using Krg.Domain.Models;

namespace Krg.Services
{
    public interface IEventRegistrationService
	{
		void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

		void RemoveRegistration(int eventId);

		List<Registration> GetAllRegistrations(int year = 0);

		List<Registration> GetNonDeletedRegistrations(int year = 0);
	}
}
