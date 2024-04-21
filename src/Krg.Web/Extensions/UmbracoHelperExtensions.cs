using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;

public static class UmbracoHelperExtensions
{
	public static IPublishedContent? SiteSettings(this UmbracoHelper umbracoHelper)
	{
		var rootNode = umbracoHelper.ContentAtRoot().FirstOrDefault();

		if (rootNode == null) return null;

		var siteSettingsNode = rootNode.FirstChildOfType("sitesettings");

		if (siteSettingsNode == null) return null;

		return siteSettingsNode;
	}

	public static IPublishedContent? EventRoot(this UmbracoHelper umbracoHelper)
	{
		var siteSettings = SiteSettings(umbracoHelper);

		if (siteSettings == null) return null;

		return siteSettings.Value<IPublishedContent>("eventroot");
	}
}
