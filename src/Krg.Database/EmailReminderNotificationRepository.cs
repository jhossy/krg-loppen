using Krg.Database.Models;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Krg.Database
{
	public class EmailReminderNotificationRepository : IEmailReminderNotificationRepository
	{
		private readonly IScopeProvider _scopeProvider;
		public EmailReminderNotificationRepository(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}

		public void AddReminder(EmailReminderNotification notification)
		{
			using var scope = _scopeProvider.CreateScope();

			scope.Database.Insert(notification);

			scope.Complete();
		}

		public void RemoveReminder(int id)
		{
			using var scope = _scopeProvider.CreateScope();

			EmailReminderNotification notificationFromDb = GetReminderById(id);

			notificationFromDb.Processed = true;

			scope.Database.Update(notificationFromDb);

			scope.Complete();
		}

		internal EmailReminderNotification GetReminderById(int id)
		{
			using var scope = _scopeProvider.CreateScope();

			var emailReminderNotification = scope.Database.SingleById<EmailReminderNotification>(id);

			scope.Complete();

			return emailReminderNotification;
		}

		public List<EmailReminderNotification> GetUnprocessedReminders()
		{
			using var scope = _scopeProvider.CreateScope();

			var emailReminderNotifications = scope.Database.Fetch<EmailReminderNotification>($"WHERE [Processed] = 0 AND [EventDate] > '{DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd")}' AND '{DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)).ToString("yyyy-MM-dd")}' > [EventDate]");

			scope.Complete();

			return emailReminderNotifications.ToList();
		}
	}
}
