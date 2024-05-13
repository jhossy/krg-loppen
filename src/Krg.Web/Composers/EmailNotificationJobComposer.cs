using Krg.Web.Jobs;
using Umbraco.Cms.Core.Composing;

namespace Krg.Web.Composers
{
    public class EmailNotificationJobComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddRecurringBackgroundJob<EmailNotificationsJob>();
		}
	}
}
