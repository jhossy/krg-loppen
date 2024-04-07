using Krg.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Krg.Database
{
    public class EventRegistrationRepository : IEventRegistrationRepository
    {
		public EventRegistrationRepository(EventRegistrationContext eventRegistrationContext)
        {
            _eventRegistrationContext = eventRegistrationContext;
        }

        public async Task AddRegistration(EventRegistration eventRegistration)
        {
            await _eventRegistrationContext.Registrations.AddAsync(eventRegistration);
        }

        public async Task<List<EventRegistration>> GetRegistrations()
        {
            return await _eventRegistrationContext.Registrations.ToListAsync();
        }

        public void RemoveRegistration(EventRegistration eventRegistration)
        {
            var registrationFound = _eventRegistrationContext.Registrations.Single(evt => evt.EventDate == eventRegistration.EventDate && evt.Email == eventRegistration.Email);
            if (registrationFound != null)
            {
                _eventRegistrationContext.Registrations.Remove(eventRegistration);
            }
        }
    }
}
