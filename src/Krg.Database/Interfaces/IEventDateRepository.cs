using Krg.Database.Models;
namespace Krg.Database.Interfaces
{
    public interface IEventDateRepository
    {
        Event GetEvent(DateTime date);
        
        List<Event> GetAllEvents(int year);
        
        void AddEvent(Event evt);
        
        void UpdateEvent(Event evt);
        
        void RemoveEvent(Event evt);
    }
}
