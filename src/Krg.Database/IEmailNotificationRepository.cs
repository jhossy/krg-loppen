using Krg.Database.Models;

namespace Krg.Database
{
	public interface IEmailNotificationRepository
	{
		void AddNotification(EmailNotification notification);

		List<EmailNotification> GetUnprocessedNotifications();
	}
}
