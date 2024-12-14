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

        public List<Event> GetAllEvents(int year)
        {
            var events = _context.Events
                .Where(x => x.Date.Year == year)
                .ToList();

            return events;
        }
    }
}
