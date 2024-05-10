﻿using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krg.Services
{
    public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly ILogger<IEventRegistrationService> _logger;

		public EventRegistrationService(
			IRegistrationRepository registrationRepository,
			ILogger<EventRegistrationService> logger)
		{
			_registrationRepository = registrationRepository;
			_logger = logger;
		}

		public void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest)
		{
			if (addRegistrationRequest == null) return;

			_registrationRepository.AddRegistration(new EventRegistration
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
				UmbracoEventNodeId = umbracoNodeId,
				UpdateTimeUtc = DateTime.UtcNow
			});
		}

		public List<Registration> GetAllRegistrations(int year)
		{
			try
			{
				return _registrationRepository
					.GetAllRegistrations(year)
					.Select(reg => new Registration(reg))
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError("GetAllRegistrations failed {@Ex}", ex);
			}

			return new List<Registration>();
		}

		public List<Registration> GetNonDeletedRegistrations(int year)
		{
			try
			{
				return _registrationRepository
					.GetNonDeletedRegistrations(year)
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
				_registrationRepository.RemoveRegistration(eventId);
			}
			catch (Exception ex) 
			{
				_logger.LogError("GetNonDeletedRegistrations failed {@Ex}", ex);
			}			
		}
	}
}
