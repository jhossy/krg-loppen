using Krg.Database.Models;
using Krg.Domain;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Services
{
    public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IScopeProvider _scopeProvider;
		public EventRegistrationService(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}

		public void AddRegistration(int umbracoNodeId, Registration eventRegistration)
		{
			using var scope = _scopeProvider.CreateScope();

			var registration = new EventRegistration
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
				UmbracoEventNodeId = umbracoNodeId,
				UpdateTimeUtc = DateTime.UtcNow				
			};

			scope.Database.Insert<EventRegistration>(registration);

			scope.Complete();
		}

		public List<Registration> GetRegistrations()
		{
			using var scope = _scopeProvider.CreateScope();

			var registrations = scope.Database.Fetch<EventRegistration>("SELECT * FROM EventRegistration");

			scope.Complete();

			return registrations.Select(reg => new Registration(reg)).ToList();
		}

		//public void RemoveRegistration(Registration eventRegistration)
		//{
		//	using var scope = _scopeProvider.CreateScope();

		//	scope.Database.Delete<EventRegistration>($"DELETE * FROM EventRegistration WHERE Email = {eventRegistration.Email} && EventDate = {eventRegistration.EventDate}");

		//	scope.Complete();
		//}
	}
}
