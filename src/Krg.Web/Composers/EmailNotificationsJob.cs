using Krg.Services.Interfaces;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Infrastructure.BackgroundJobs;

namespace Krg.Web.Composers
{
	public class EmailNotificationJobComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddRecurringBackgroundJob<EmailNotificationsJob>();
		}
	}

	public class EmailNotificationsJob : IRecurringBackgroundJob
	{
		public TimeSpan Period => TimeSpan.FromSeconds(30);

		public TimeSpan Delay => TimeSpan.FromSeconds(3);

		public event EventHandler PeriodChanged { add { } remove { } }

		private readonly IEmailservice _emailService;

        public EmailNotificationsJob(IEmailservice emailservice)
        {
            _emailService = emailservice;
        }

        public async Task RunJobAsync()
		{
			Console.WriteLine("Executing EmailNotificationsJob...");

			await _emailService.SendEmail("sender@spejderknud.dk", new[] { "receiver@knud.dk" }, "tilmelding", "du er tilmeldt");

			return;
		}
	}
}
