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

        public EventDate GetEventById(int id)
        {
	        var eventInDb = _unitOfWork.EventRepository.GetEventById(id);

	        if (eventInDb == null) return null;
	        
	        return new EventDate
	        {
		        Id = eventInDb.Id,
		        Date = eventInDb.Date, 
		        ContactEmail = eventInDb.ContactEmail, 
		        ContactPhone = eventInDb.ContactPhone, 
		        ContactName = eventInDb.ContactName,
		        Note = eventInDb.Note
	        };
        }

        public EventDate GetEventByDate(DateTime date)
        {
	        var eventInDb = _unitOfWork.EventRepository.GetEvent(date);

	        if (eventInDb == null) return null;
	        
	        return new EventDate
	        {
		        Id = eventInDb.Id,
		        Date = eventInDb.Date, 
		        ContactEmail = eventInDb.ContactEmail, 
		        ContactPhone = eventInDb.ContactPhone, 
		        ContactName = eventInDb.ContactName,
		        Note = eventInDb.Note
	        };
        }

        public List<EventDate> GetEvents(int year)
		{
			return _unitOfWork.EventRepository
					.GetAllEvents(year)
					.Select(x =>
						new EventDate
						{
							Id = x.Id,
							Date = x.Date,
							ContactEmail = x.ContactEmail,
							ContactName = x.ContactName,
							ContactPhone = x.ContactPhone,
							Note = x.Note
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
				ContactEmail = eventDate.ContactEmail,
				Note = eventDate.Note
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
				ContactEmail = eventDate.ContactEmail,
				Note = eventDate.Note
			});
			
			_unitOfWork.Commit();
		}

		public void RemoveEventDate(int id)
		{
			_unitOfWork.EventRepository.RemoveEvent(id);
			
			_unitOfWork.Commit();
		}
	}
}
