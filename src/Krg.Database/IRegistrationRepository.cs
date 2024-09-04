using Krg.Database.Models;

namespace Krg.Database
{
	public interface IRegistrationRepository
	{
		int AddRegistration(EventRegistration registration);

		EventRegistration GetById(int id);

		void RemoveRegistration(int id);

		List<EventRegistration> GetAllRegistrations(int year);

		List<EventRegistration> GetNonDeletedRegistrations(int year);

		void UpdateRegistration(int id, string name);
	}
}
