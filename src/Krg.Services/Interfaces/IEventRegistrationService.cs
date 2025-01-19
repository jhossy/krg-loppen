using Krg.Database.Models;
using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEventRegistrationService
    {
		int AddRegistration(AddRegistrationRequest addRegistrationRequest);

        EventRegistration GetById(int id);

		void RemoveRegistration(int eventId);

        List<Registration> GetAllRegistrations(DateRange dateRange);

        List<Registration> GetNonDeletedRegistrations(DateRange dateRange);
    }
}
