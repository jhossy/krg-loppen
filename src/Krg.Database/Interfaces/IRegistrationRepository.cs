using Krg.Database.Models;

namespace Krg.Database.Interfaces
{
    public interface IRegistrationRepository
    {
        int AddRegistration(EventRegistration registration);

        EventRegistration GetById(int id);

        void RemoveRegistration(int id);

        List<EventRegistration> GetAllRegistrations(DateRange dateRange);

        List<EventRegistration> GetNonDeletedRegistrations(DateRange dateRange);
    }
}
