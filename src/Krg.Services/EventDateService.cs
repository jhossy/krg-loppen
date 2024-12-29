using Krg.Database.Interfaces;
using Krg.Database.Models;
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

        public EventDate GetEventByDate(DateTime date)
        {
	        var eventInDb = _unitOfWork.EventRepository.GetEvent(date);

	        if (eventInDb == null) return null;
	        
	        return new EventDate
	        {
		        Date = eventInDb.Date, 
		        ContactEmail = eventInDb.ContactEmail, 
		        ContactPhone = eventInDb.ContactPhone, 
		        ContactName = eventInDb.ContactName
	        };
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

		public void AddEventDate(EventDate eventDate)
		{
			_unitOfWork.EventRepository.AddEvent(new Event
			{
				Date = eventDate.Date,
				ContactName = eventDate.ContactName,
				ContactPhone = eventDate.ContactPhone,
				ContactEmail = eventDate.ContactEmail
			});
			
			_unitOfWork.Commit();
		}

		public void UpdateEventDate(EventDate eventDate)
		{
			_unitOfWork.EventRepository.UpdateEvent(new Event
			{
				Date = eventDate.Date,
				ContactName = eventDate.ContactName,
				ContactPhone = eventDate.ContactPhone,
				ContactEmail = eventDate.ContactEmail
			});
			
			_unitOfWork.Commit();
		}

		public void RemoveEventDate(EventDate eventDate)
		{
			_unitOfWork.EventRepository.RemoveEvent(new Event{Date = eventDate.Date});
			
			_unitOfWork.Commit();
		}
	}
}
