﻿using Krg.Database;
using Krg.Web.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace Krg.Web.Composers
{
    public class EventRegistrationsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunEventRegistrationsMigration>();
        }
    }

    public class EventContentRegistrationsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<SendingContentNotification, EventContentNotificationHandler>();
        }
    }
}
