using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace Krg.Database
{
	public class EmailNotificationsComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunEmailNotificationsMigration>();
		}
	}
}
