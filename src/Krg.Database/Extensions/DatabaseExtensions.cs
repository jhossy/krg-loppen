using Krg.Database.Interfaces;
using Krg.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Krg.Database.Extensions
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection AddDatabaseExtensions(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
			services.AddTransient<IEmailReminderNotificationRepository, EmailReminderNotificationRepository>();

			return services;
		}
	}
}
