using Krg.Web.Jobs;
using Umbraco.Cms.Core.Composing;

namespace Krg.Web.Composers
{
	public class EmailReminderNotificationJobComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddRecurringBackgroundJob<EmailReminderNotificationsJob>();
		}
	}
}
