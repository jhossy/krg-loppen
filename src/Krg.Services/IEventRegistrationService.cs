using Krg.Domain;

namespace Krg.Services
{
	public interface IEventRegistrationService
	{
		void AddRegistration(int umbracoNodeId, Registration eventRegistration);

		//void RemoveRegistration(Registration eventRegistration);

		List<Registration> GetRegistrations();
	}
}
