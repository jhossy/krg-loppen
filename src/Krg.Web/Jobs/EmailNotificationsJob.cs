using Krg.Services.Interfaces;
using Umbraco.Cms.Infrastructure.BackgroundJobs;

namespace Krg.Web.Jobs
{

    public class EmailNotificationsJob : IRecurringBackgroundJob
    {
        public TimeSpan Period => TimeSpan.FromMinutes(5);

        public TimeSpan Delay => TimeSpan.FromMinutes(3);

        public event EventHandler PeriodChanged { add { } remove { } }

        private readonly IEmailservice _emailService;
        private readonly IEmailNotificationService _notificationService;
        private readonly ILogger<EmailNotificationsJob> _logger;

        public EmailNotificationsJob(
            IEmailservice emailservice,
            IEmailNotificationService notificationService,
            ILogger<EmailNotificationsJob> logger)
        {
            _emailService = emailservice;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task RunJobAsync()
        {
            Console.WriteLine("Executing EmailNotificationsJob...");

            _logger.LogInformation("Executing EmailNotificationsJob...");

            var nonProcessedNotifications = _notificationService.GetNonProcessedNotifications();

            if (!nonProcessedNotifications.Any())
            {
                _logger.LogInformation("Finished EmailNotificationsJob - zero unprocessed notifications in queue");
                return;
            }

            foreach (var notification in nonProcessedNotifications)
            {
                await _emailService.SendEmail("support@spejderknud.dk", new[] { notification.To }, notification.Subject, notification.Body);

                _notificationService.RemoveNotification(notification.Id);
            }

            _logger.LogInformation("Finished EmailNotificationsJob.");

            return;
        }
    }
}
