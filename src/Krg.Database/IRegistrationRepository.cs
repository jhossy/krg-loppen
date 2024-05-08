﻿using Krg.Database.Models;

namespace Krg.Database
{
	public interface IRegistrationRepository
	{
		void AddRegistration(EventRegistration registration);

		EventRegistration GetById(int id);

		void RemoveRegistration(int id);

		List<EventRegistration> GetAllRegistrations(int year = 0);

		List<EventRegistration> GetNonDeletedRegistrations(int year = 0);
	}
}
