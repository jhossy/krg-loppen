using Krg.Database.Models;

namespace Krg.Database
{
	public interface IEmailNotificationRepository
	{
		void AddNotification(EmailNotification notification);
				
		void RemoveNotification(int id);
		
		List<EmailNotification> GetUnprocessedNotifications();
	}
}
