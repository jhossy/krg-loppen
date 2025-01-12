using Krg.Database.Interfaces;
using Krg.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krg.Database.Extensions
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection AddDatabaseExtensions(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("KrgContext");
			
			services.AddDbContext<KrgContext>(
				options => options.UseSqlServer(
					connectionString,
					o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName)));
	
			services.AddDbContext<IdentityContext>(
				options => options.UseSqlServer(
					connectionString,
					o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "identity")));
			
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
			services.AddTransient<IEmailReminderNotificationRepository, EmailReminderNotificationRepository>();

			return services;
		}
	}
}
