using Krg.Services.Interfaces;
using Polly.Retry;
using Polly;
using Umbraco.Cms.Infrastructure.BackgroundJobs;

namespace Krg.Web.Jobs
{
	public class EmailReminderNotificationsJob : IRecurringBackgroundJob
    {
        public TimeSpan Period => TimeSpan.FromHours(12);
        public TimeSpan Delay => TimeSpan.FromMinutes(5);
        
        public event EventHandler PeriodChanged { add { } remove { } }

        private readonly IEmailservice _emailService;
        private readonly IEmailNotificationService _notificationService;
        private readonly ILogger<EmailReminderNotificationsJob> _logger;
        private readonly ResiliencePipeline _pipeline;
        
        public EmailReminderNotificationsJob(
            IEmailservice emailservice,
            IEmailNotificationService notificationService,
            ILogger<EmailReminderNotificationsJob> logger)
        {
            _emailService = emailservice;
            _notificationService = notificationService;
            _logger = logger;

            _pipeline = new ResiliencePipelineBuilder()
                            .AddRetry(new RetryStrategyOptions()
                            {
                                BackoffType = DelayBackoffType.Exponential,
                                Delay = TimeSpan.FromSeconds(3),
                                MaxRetryAttempts = 2,
                                UseJitter = true,
                                OnRetry = args =>
                                {
                                    _logger.LogWarning("OnRetry, Attempt: {0}", args.AttemptNumber);

                                    return default;
                                }
                            }) // Add retry using the default options
                            .AddTimeout(TimeSpan.FromSeconds(10)) // Add 10 seconds timeout
                            .Build(); // Builds the resilience pipeline
		}

		public async Task RunJobAsync()
        {
            Console.WriteLine("Executing EmailReminderNotificationsJob...");

            _logger.LogInformation("Executing EmailReminderNotificationsJob...");

            var nonProcessedReminders = _notificationService.GetNonProcessedReminders();

            if (!nonProcessedReminders.Any())
            {
                _logger.LogInformation("Finished EmailReminderNotificationsJob - zero unprocessed reminders in queue");
                return;
            }

            foreach (var reminder in nonProcessedReminders)
			{
                await _pipeline.ExecuteAsync(async token => 
                {
                    await _emailService.SendEmail("support@spejderknud.dk", new[] { reminder.To }, reminder.Subject, reminder.Body);
                });

                _notificationService.RemoveReminder(reminder.Id);
            }

            _logger.LogInformation("Finished EmailReminderNotificationsJob.");

            return;
        }
    }
}
