using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEventRegistrationService
    {
        void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest);

        void RemoveRegistration(int eventId);

        List<Registration> GetAllRegistrations(int year);

        List<Registration> GetNonDeletedRegistrations(int year);
    }
}
