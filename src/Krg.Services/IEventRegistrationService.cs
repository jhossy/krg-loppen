using Krg.Domain;

namespace Krg.Services
{
	public interface IEventRegistrationService
	{
		void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

		List<RegistrationViewModel> GetRegistrations();
	}
}
