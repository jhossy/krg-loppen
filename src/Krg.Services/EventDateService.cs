using Krg.Database.Interfaces;
using Krg.Domain.Models;
using Krg.Services.Interfaces;

namespace Krg.Services
{

	public class EventDateService : IEventDateService
	{
		private readonly IUnitOfWork _unitOfWork;
        public EventDateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<EventDate> GetEvents(int year)
		{
			return _unitOfWork.EventRepository
					.GetAllEvents(year)
					.Select(x =>
						new EventDate
						{
							Date = x.Date,
							ContactEmail = x.ContactEmail,
							ContactName = x.ContactName,
							ContactPhone = x.ContactPhone,
						})
					.ToList();
		}
	}
}
