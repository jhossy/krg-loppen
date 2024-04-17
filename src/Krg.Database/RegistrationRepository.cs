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

		public void AddRegistration(EventRegistration registration)
		{
			using var scope = _scopeProvider.CreateScope();
												
			scope.Database.Insert<EventRegistration>(registration);

			scope.Complete();
		}

		public List<EventRegistration> GetRegistrations()
		{
			using var scope = _scopeProvider.CreateScope();

			var registrations = scope.Database.Fetch<EventRegistration>("SELECT * FROM EventRegistration");

			scope.Complete();

			return registrations.ToList();
		}
	}
}
