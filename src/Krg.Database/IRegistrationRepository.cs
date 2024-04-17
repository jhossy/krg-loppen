using Krg.Database.Models;

namespace Krg.Database
{
	public interface IRegistrationRepository
	{
		void AddRegistration(EventRegistration registration);

		List<EventRegistration> GetRegistrations();
	}
}
