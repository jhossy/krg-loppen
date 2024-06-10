using Krg.Database.Models;

namespace Krg.Database
{
	public interface IEmailReminderNotificationRepository
	{
		void AddReminder(EmailReminderNotification notification);

		void RemoveReminder(int id);

		void CancelReminder(int umbracoNodeId);

		List<EmailReminderNotification> GetUnprocessedReminders();
	}
}
