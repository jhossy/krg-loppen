using Krg.Database.Models;
namespace Krg.Database.Interfaces
{
    public interface IEventDateRepository
    {
        Event GetEventById(int id);
        
        Event GetEvent(DateTime date);
        
        List<Event> GetAllEvents(int year);
        
        void AddEvent(Event evt);
        
        void UpdateEvent(Event evt);
        
        void RemoveEvent(int id);
    }
}
