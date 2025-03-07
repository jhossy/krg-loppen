﻿using Krg.Database.Interfaces;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krg.Services
{
    public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<IEventRegistrationService> _logger;

		public EventRegistrationService(
			IUnitOfWork unitOfWork,
			ILogger<EventRegistrationService> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public int AddRegistration(AddRegistrationRequest addRegistrationRequest)
		{
			if (addRegistrationRequest == null) return 0;

			var registrationToAdd = new EventRegistration
            {
                BringsTrailer = addRegistrationRequest.BringsTrailer,
                Department = addRegistrationRequest.Department,
                Email = addRegistrationRequest.Email,
                EventDate = addRegistrationRequest.EventDate,
                Name = addRegistrationRequest.Name,
                NoOfAdults = addRegistrationRequest.NoOfAdults,
                NoOfChildren = addRegistrationRequest.NoOfChildren,
                PhoneNo = addRegistrationRequest.PhoneNo,
                ShowName = addRegistrationRequest.ShowName,
                UmbracoEventNodeId = 0, //todo remove
                UpdateTimeUtc = DateTime.UtcNow
            };

            _unitOfWork.RegistrationRepository.AddRegistration(registrationToAdd);				

			_unitOfWork.Commit();

			return registrationToAdd.Id;
		}

		public List<Registration> GetAllRegistrations(DateRange dateRange)
		{
			try
			{
				return _unitOfWork.RegistrationRepository
					.GetAllRegistrations(dateRange)
					.Select(reg => new Registration(reg))
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError("GetAllRegistrations failed {@Ex}", ex);
			}

			return new List<Registration>();
		}

		public EventRegistration GetById(int id)
		{
			try
			{
				return _unitOfWork.RegistrationRepository.GetById(id);				
			}
			catch (Exception ex)
			{
				_logger.LogError("GetById failed {@Ex}", ex);
			}

			return null;
		}

		public List<Registration> GetNonDeletedRegistrations(DateRange dateRange)
		{
			try
			{
				return _unitOfWork.RegistrationRepository
					.GetNonDeletedRegistrations(dateRange)
					.Select(reg => new Registration(reg))
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError("GetNonDeletedRegistrations failed {@Ex}", ex);
			}
			return new List<Registration>();
		}

		public void RemoveRegistration(int eventId)
		{
			try
			{
				_unitOfWork.RegistrationRepository.RemoveRegistration(eventId);

				_unitOfWork.Commit();
			}
			catch (Exception ex) 
			{
				_logger.LogError("GetNonDeletedRegistrations failed {@Ex}", ex);
			}			
		}
	}
}
