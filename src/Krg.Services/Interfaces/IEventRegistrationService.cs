using Krg.Database.Models;
using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEventRegistrationService
    {
		int AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

        EventRegistration GetById(int id);

		void RemoveRegistration(int eventId);

        List<Registration> GetAllRegistrations(int year);

        List<Registration> GetNonDeletedRegistrations(int year);
    }
}
