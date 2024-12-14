using Krg.Database.Interfaces;
using Krg.Database.Models;

namespace Krg.Database.Repositories
{

    public class EmailNotificationRepository : IEmailNotificationRepository
    {
        private readonly KrgContext _context;

        public EmailNotificationRepository(KrgContext context)
        {
            _context = context;
        }

        public void AddNotification(EmailNotification notification)
        {
            _context.EmailNotifications.Add(notification);
        }

        public void RemoveNotification(int id)
        {
            EmailNotification notificationFromDb = GetById(id);

            notificationFromDb.Processed = true;

            _context.EmailNotifications.Update(notificationFromDb);
        }

        internal EmailNotification GetById(int id)
        {
            var emailNotification = _context.EmailNotifications.Single(x => x.Id == id);

            return emailNotification;
        }

        public List<EmailNotification> GetUnprocessedNotifications()
        {
            var notifications = _context.EmailNotifications
                .Where(x => x.Processed == false)
                .ToList();

            return notifications;
        }
    }
}
