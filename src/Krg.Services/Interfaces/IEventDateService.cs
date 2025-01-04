using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEventDateService
    {
        EventDate GetEventById(int id);
        
        EventDate GetEventByDate(DateTime date);
        
        List<EventDate> GetEvents(int year);
        
        void AddEventDate(EventDate eventDate);
        
        void UpdateEventDate(EventDate eventDate);
        
        void RemoveEventDate(int id);
    }
}
