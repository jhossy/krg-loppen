using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEventDateService
    {
        List<EventDate> GetEvents(int year);
    }
}
