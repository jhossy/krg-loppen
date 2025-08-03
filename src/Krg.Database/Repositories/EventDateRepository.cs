using Krg.Database.Interfaces;
using Krg.Database.Models;
namespace Krg.Database.Repositories
{
    public class EventDateRepository : IEventDateRepository
    {
        private readonly KrgContext _context;
        public EventDateRepository(KrgContext context)
        {
            _context = context;
        }
        
        public Event GetEventById(int id)
        {
            return _context.Events.Single(x => x.Id == id);
        }

        public Event GetEvent(DateTime date)
        {
            return _context.Events.FirstOrDefault(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day);
        }

        public List<Event> GetAllEvents(int year)
        {
            var events = _context.Events
                .Where(x => x.Date.Year == year)
                .ToList();

            return events;
        }

        public void AddEvent(Event evt)
        {
            evt.UpdateTimeUtc = DateTime.UtcNow;
            
            _context.Events.Add(evt);
        }

        public void UpdateEvent(Event evt)
        {
            var eventToUpdate = GetEvent(evt.Date);

            if (eventToUpdate != null)
            {
                eventToUpdate.ContactName = evt.ContactName;
                eventToUpdate.ContactPhone = evt.ContactPhone;
                eventToUpdate.ContactEmail = evt.ContactEmail;
                eventToUpdate.UpdateTimeUtc = DateTime.UtcNow;
                eventToUpdate.Note = evt.Note;
                
                _context.Events.Update(eventToUpdate);
            }
        }

        public void RemoveEvent(int id)
        {
            var eventToRemove = GetEventById(id);

            if (eventToRemove == null) return;

            _context.Events.Remove(eventToRemove);
        }
    }
}
