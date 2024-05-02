using Krg.Domain.Models;

namespace Krg.Services
{
    public interface IEventRegistrationService
	{
		void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

		void RemoveRegistration(int eventId);

		List<Registration> GetAllRegistrations();

		List<Registration> GetNonDeletedRegistrations();
	}
}
