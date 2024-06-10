using Krg.Database.Models;
using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        void AddNotification(AddRegistrationRequest registrationRequest, string emailSender);

        void AddReminder(AddRegistrationRequest registrationRequest, string emailSender, int eventRegistrationId);

		void RemoveNotification(int id);

        void RemoveReminder(int id);

        void CancelReminder(int umbracoNodeId);

		List<Notification> GetNonProcessedNotifications();
        List<EmailReminder> GetNonProcessedReminders();

	}
}
