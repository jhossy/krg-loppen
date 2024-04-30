using Krg.Domain.Models;

namespace Krg.Services
{
    public interface IEventRegistrationService
	{
		void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

		List<Registration> GetRegistrations();
	}
}
