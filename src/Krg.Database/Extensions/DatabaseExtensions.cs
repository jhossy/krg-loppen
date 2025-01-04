using Krg.Database.Interfaces;
using Krg.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krg.Database.Extensions
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection AddDatabaseExtensions(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<KrgContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("KrgContext")));
	
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("KrgContext")));
			
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
			services.AddTransient<IEmailReminderNotificationRepository, EmailReminderNotificationRepository>();

			return services;
		}
	}
}
