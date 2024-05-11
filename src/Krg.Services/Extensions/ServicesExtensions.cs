using Krg.Database;
using Krg.Services.Interfaces;
using Krg.Web.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Krg.Services.Extensions
{
    public static class ServicesExtensions
	{
		public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
		{
			services.AddTransient<IEventRegistrationService, EventRegistrationService>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IExcelService, ExcelService>();
			services.AddTransient<IEmailNotificationService, EmailNotificationService>();
			services.AddTransient<INotificationRepository, NotificationRepository>();
			return services;
		}
	}
}
