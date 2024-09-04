using Krg.Database.Models;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Database
{
	public class RegistrationRepository : IRegistrationRepository
	{
		private readonly IScopeProvider _scopeProvider;
		public RegistrationRepository(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}

		public int AddRegistration(EventRegistration registration)
		{
			using var scope = _scopeProvider.CreateScope();
												
			scope.Database.Insert(registration);

			scope.Complete();

			return registration.Id;
		}

		public void UpdateRegistration(EventRegistration registration)
		{
			using var scope = _scopeProvider.CreateScope();

			EventRegistration eventRegistration = GetById(registration.Id);

			if (eventRegistration != null)
			{
				eventRegistration.BringsTrailer = registration.BringsTrailer;
				eventRegistration.Department = registration.Department;
				eventRegistration.EventDate = registration.EventDate;
				eventRegistration.IsCancelled = registration.IsCancelled;
				eventRegistration.Name = registration.Name;
				eventRegistration.NoOfAdults = registration.NoOfAdults;
				eventRegistration.NoOfChildren = registration.NoOfChildren;
				eventRegistration.PhoneNo = registration.PhoneNo;
				eventRegistration.ShowName = registration.ShowName;
				eventRegistration.UmbracoEventNodeId = registration.UmbracoEventNodeId;
				eventRegistration.UpdateTimeUtc = registration.UpdateTimeUtc;

				scope.Database.Update(eventRegistration);
			}
            scope.Complete();
		}

		public EventRegistration GetById(int id)
		{
			using var scope = _scopeProvider.CreateScope();

			var registration = scope.Database.SingleById<EventRegistration>(id);

			scope.Complete();

			return registration;
		}
	
		public List<EventRegistration> GetAllRegistrations(int year)
		{
			using var scope = _scopeProvider.CreateScope();

			var registrations = scope.Database.Fetch<EventRegistration>($"WHERE year(EventDate) = {year}");

			scope.Complete();

			return registrations.ToList();
		}

		public List<EventRegistration> GetNonDeletedRegistrations(int year)
		{
			using var scope = _scopeProvider.CreateScope();

			var registrations = scope.Database.Fetch<EventRegistration>($"WHERE year(EventDate) = {year} AND IsCancelled=0");

			scope.Complete();

			return registrations.ToList();
		}

		public void RemoveRegistration(int id)
		{
			EventRegistration eventFound = GetById(id);

			if (eventFound == null) return;

			eventFound.IsCancelled = true;

			UpdateRegistration(eventFound);
		}

		public void UpdateRegistration(int id, string name)
		{
			EventRegistration eventFound = GetById(id);

			if (eventFound == null) return;

			eventFound.Name = name;

			UpdateRegistration(eventFound);
		}
	}
}
