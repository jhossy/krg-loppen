using Krg.Database.Interfaces;
using Krg.Database.Models;

namespace Krg.Database.Repositories
{
    public class EmailReminderNotificationRepository : IEmailReminderNotificationRepository
    {
        private readonly KrgContext _context;
        public EmailReminderNotificationRepository(KrgContext context)
        {
            _context = context;
        }

        public void AddReminder(EmailReminderNotification notification)
        {
            _context.EmailReminderNotifications.Add(notification);
        }

        public void SetIsProcessed(int id)
        {
            EmailReminderNotification notificationFromDb = GetReminderById(id);

            notificationFromDb.Processed = true;

            _context.EmailReminderNotifications.Update(notificationFromDb);
        }

        internal EmailReminderNotification GetReminderById(int id)
        {
            var emailReminderNotification = _context.EmailReminderNotifications.Single(x => x.Id == id);

            return emailReminderNotification;
        }

        public List<EmailReminderNotification> GetUnprocessedReminders()
        {
            var emailReminderNotifications = _context.EmailReminderNotifications
                .Where(x =>
                    x.Processed == false &&
                    x.IsCancelled == false &&
                    x.EventDate > DateTime.UtcNow && DateTime.UtcNow.AddDays(3) > x.EventDate)
                .ToList();

            return emailReminderNotifications;
        }

        public void CancelReminder(int eventRegistrationId)
        {
            var reminderFromDb = _context.EmailReminderNotifications.FirstOrDefault(x => x.FkEventRegistrationId == eventRegistrationId);

            if (reminderFromDb == null) return;

            reminderFromDb.IsCancelled = true;

            UpdateReminder(reminderFromDb);
        }

        internal void UpdateReminder(EmailReminderNotification reminder)
        {
            _context.EmailReminderNotifications.Update(reminder);
        }
    }
}
