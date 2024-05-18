using Krg.Domain;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

public static class UmbracoHelperExtensions
{
	public static SiteSettings? SiteSettings(this UmbracoHelper umbracoHelper)
	{
		var rootNode = umbracoHelper.ContentAtRoot().FirstOrDefault();

		if (rootNode == null) return null;

		var siteSettingsNode = rootNode.FirstChild<SiteSettings>();		

		if (siteSettingsNode == null) return null;

		return siteSettingsNode;
	}

	public static EventRoot? EventRoot(this UmbracoHelper umbracoHelper)
	{
		var siteSettings = SiteSettings(umbracoHelper);

		if (siteSettings == null) return null;

		return siteSettings.Value<EventRoot>("eventroot");
	}

	public static string GetEmailSenderOrFallback(this UmbracoHelper umbracoHelper)
	{
		var siteSettings = SiteSettings(umbracoHelper);

		if (siteSettings == null || string.IsNullOrEmpty(siteSettings.EmailSender))
			return Constants.Email.EmailSender;

		return siteSettings.EmailSender;
	}
}
