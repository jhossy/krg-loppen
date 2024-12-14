using Krg.Database.Models;
namespace Krg.Database.Interfaces
{
    public interface IEventDateRepository
    {
        List<Event> GetAllEvents(int year);
    }
}
