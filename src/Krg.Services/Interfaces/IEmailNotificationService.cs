using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        void AddNotification(AddRegistrationRequest registrationRequest, string emailSender);

        void AddReminder(AddRegistrationRequest registrationRequest, string emailSender);

		void RemoveNotification(int id);

        void RemoveReminder(int id);

		List<Notification> GetNonProcessedNotifications();
        List<EmailReminder> GetNonProcessedReminders();

	}
}
