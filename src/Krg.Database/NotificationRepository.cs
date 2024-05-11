using Krg.Database.Models;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Database
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IScopeProvider _scopeProvider;
		public NotificationRepository(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}
		
		public void AddNotification(EmailNotification notification)
		{
			using var scope = _scopeProvider.CreateScope();
			
			scope.Database.Insert(notification);
			
			scope.Complete();
		}

		public List<EmailNotification> GetUnprocessedNotifications()
		{
			using var scope = _scopeProvider.CreateScope();

			var notifications = scope.Database.Fetch<EmailNotification>($"WHERE Processed = 0");

			scope.Complete();

			return notifications.ToList();
		}
	}
}
