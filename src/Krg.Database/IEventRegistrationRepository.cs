using Krg.Database.Models;

namespace Krg.Database
{
    public interface IEventRegistrationRepository
    {
        public Task AddRegistration(EventRegistration eventRegistration);

        public void RemoveRegistration(EventRegistration eventRegistration);

        public Task<List<EventRegistration>> GetRegistrations();
    }
}
