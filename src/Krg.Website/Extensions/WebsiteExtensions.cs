using Krg.Website.Models;

namespace Krg.Website.Extensions
{
	public static class WebsiteExtensions
	{
		public static IServiceCollection AddWebsiteExtensions(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

			return services;
		}
	}
}
