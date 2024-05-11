using Krg.Database.Models;

namespace Krg.Database
{
	public interface INotificationRepository
	{
		void AddNotification(EmailNotification notification);

		List<EmailNotification> GetUnprocessedNotifications();
	}
}
