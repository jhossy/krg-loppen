﻿using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace Krg.Web.NotificationHandlers
{
	public class EventContentNotificationHandler : INotificationHandler<SendingContentNotification>
	{
		public void Handle(SendingContentNotification notification)
		{
			if (notification.Content.ContentTypeAlias.Equals("eventroot"))
			{

				// Access the property you want to pre-populate
				// each content item can have 'variations' - each variation is represented by the `ContentVariantDisplay` class.
				// if your site uses variants, then you need to decide whether to set the default value for all variants or a specific variant
				// eg. set by variant name:
				// var variant = notification.Content.Variants.FirstOrDefault(f => f.Name == "specificVariantName");
				// OR loop through all the variants:
				foreach (var variant in notification.Content.Variants)
				{
					// Check if variant is a 'new variant'
					// we only want to set the default value when the content item is first created
					if (variant.State == ContentSavedState.NotCreated)
					{
						// each variant has an IEnumerable of 'Tabs' (property groupings)
						// and each of these contain an IEnumerable of `ContentPropertyDisplay` properties
						// find the first property with alias 'publishDate'
						var pubDateProperty = variant.Tabs.SelectMany(f => f.Properties)
							.FirstOrDefault(f => f.Alias.InvariantEquals("exportyear"));
						if (pubDateProperty is not null)
						{
							// set default value of the publish date property if it exists
							pubDateProperty.Value = DateTime.UtcNow.Year;
						}
					}
				}
			}
		}
	}
}
