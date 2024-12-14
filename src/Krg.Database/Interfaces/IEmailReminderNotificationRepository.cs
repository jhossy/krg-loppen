using Krg.Database.Models;

namespace Krg.Database.Interfaces
{
    public interface IEmailReminderNotificationRepository
    {
        void AddReminder(EmailReminderNotification notification);

        void SetIsProcessed(int id);

        void CancelReminder(int eventRegistrationId);

        List<EmailReminderNotification> GetUnprocessedReminders();
    }
}
