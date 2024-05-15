using Krg.Services.Interfaces;
using Polly.Retry;
using Polly;
using Umbraco.Cms.Infrastructure.BackgroundJobs;
using System.Threading;

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
        private readonly ResiliencePipeline _pipeline;
        
        public EmailNotificationsJob(
            IEmailservice emailservice,
            IEmailNotificationService notificationService,
            ILogger<EmailNotificationsJob> logger)
        {
            _emailService = emailservice;
            _notificationService = notificationService;
            _logger = logger;

			_pipeline = new ResiliencePipelineBuilder()
	                        .AddRetry(new RetryStrategyOptions() 
                            {
							    BackoffType = DelayBackoffType.Exponential,
							    Delay = TimeSpan.FromSeconds(3),
							    MaxRetryAttempts = 3,
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
                await _pipeline.ExecuteAsync(async token => 
                {
                    await _emailService.SendEmail("support@spejderknud.dk", new[] { notification.To }, notification.Subject, notification.Body);
                });

                _notificationService.RemoveNotification(notification.Id);
            }

            _logger.LogInformation("Finished EmailNotificationsJob.");

            return;
        }
    }
}
