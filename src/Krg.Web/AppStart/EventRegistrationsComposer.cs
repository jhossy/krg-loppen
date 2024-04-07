using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace Krg.Database
{
	public class EventRegistrationsComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunEventRegistrationsMigration>();
		}
	}
}
