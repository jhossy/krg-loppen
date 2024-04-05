using Krg.Domain;

namespace Krg.Services
{
	public interface IEventRegistrationService
	{
		Task AddRegistration(Registration eventRegistration);

		void RemoveRegistration(Registration eventRegistration);

		Task<List<Registration>> GetRegistrations();
	}
}
