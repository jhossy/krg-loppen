using Krg.Database.Models;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Database
{
	public class EmailNotificationRepository : IEmailNotificationRepository
	{
		private readonly IScopeProvider _scopeProvider;
		public EmailNotificationRepository(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}
		
		public void AddNotification(EmailNotification notification)
		{
			using var scope = _scopeProvider.CreateScope();
			
			scope.Database.Insert(notification);
			
			scope.Complete();
		}

		public void RemoveNotification(int id)
		{
			using var scope = _scopeProvider.CreateScope();

			EmailNotification notificationFromDb = GetById(id);
			
			notificationFromDb.Processed = true;

			scope.Database.Update(notificationFromDb);

			scope.Complete();
		}

		internal EmailNotification GetById(int id)
		{
			using var scope = _scopeProvider.CreateScope();

			var emailNotification = scope.Database.SingleById<EmailNotification>(id);

			scope.Complete();

			return emailNotification;
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
