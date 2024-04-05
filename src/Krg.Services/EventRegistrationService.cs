using Krg.Database;
using Krg.Database.Models;
using Krg.Domain;

namespace Krg.Services
{
    public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEventRegistrationRepository _eventRegistrationRepository;

        public EventRegistrationService(IUnitOfWork unitOfWork, IEventRegistrationRepository eventRegistrationRepository)
        {
            _unitOfWork = unitOfWork;
			_eventRegistrationRepository = eventRegistrationRepository;
        }

		public async Task AddRegistration(Registration eventRegistration)
		{
			await _eventRegistrationRepository.AddRegistration(new EventRegistration
			{
                BringsTrailer = eventRegistration.BringsTrailer,
                Department = eventRegistration.Department,
                Email = eventRegistration.Email,
                EventDate = eventRegistration.EventDate,
                Name = eventRegistration.Name,
                NoOfAdults = eventRegistration.NoOfAdults,
                NoOfChildren = eventRegistration.NoOfChildren,
                PhoneNo = eventRegistration.PhoneNo,
                ShowName = eventRegistration.ShowName,
            });

			_unitOfWork.Commit();
		}

		public async Task<List<Registration>> GetRegistrations()
		{
			var registrations = await _eventRegistrationRepository.GetRegistrations();

			return registrations.Select(reg => new Registration(reg)).ToList();
		}

		public void RemoveRegistration(Registration eventRegistration)
		{
			_eventRegistrationRepository.RemoveRegistration(new EventRegistration
            {
                BringsTrailer = eventRegistration.BringsTrailer,
                Department = eventRegistration.Department,
                Email = eventRegistration.Email,
                EventDate = eventRegistration.EventDate,
                Name = eventRegistration.Name,
                NoOfAdults = eventRegistration.NoOfAdults,
                NoOfChildren = eventRegistration.NoOfChildren,
                PhoneNo = eventRegistration.PhoneNo,
                ShowName = eventRegistration.ShowName,
            });

			_unitOfWork.Commit();
		}
	}
}
