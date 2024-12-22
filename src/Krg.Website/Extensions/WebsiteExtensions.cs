using Krg.Website.Jobs;
using Krg.Website.Models;
using Quartz;

namespace Krg.Website.Extensions
{
	public static class WebsiteExtensions
	{
		public static IServiceCollection AddWebsiteExtensions(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

			return services;
		}

		public static IServiceCollection AddScheduledJobs(this IServiceCollection services)
		{
			services.AddQuartz(q =>
			{
				var jobKey = new JobKey(nameof(EmailNotificationsJob));

				q.AddJob<EmailNotificationsJob>(jobKey)
				 .AddTrigger(opts => opts.ForJob(jobKey)
					.WithIdentity("EmailNotificationsJob-trigger")
					.WithCronSchedule("0 * * ? * *"))
					.UsePersistentStore(s =>
					{

						s.RetryInterval = TimeSpan.FromSeconds(5);
						s.UseMicrosoftSQLite(
							options =>
							{
								options.ConnectionStringName = "Jobs";
							}
						);
						s.UseNewtonsoftJsonSerializer();
						//s.UseSqlServer(sqlserver =>
						//{
						//	sqlserver.ConnectionString = "Server=.\\sqlexpress;Database=quartz-sample;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
						//});
					});

				var emailReminderJobKey = new JobKey(nameof(EmailReminderNotificationsJob));

				q.AddJob<EmailReminderNotificationsJob>(emailReminderJobKey)
				 .AddTrigger(opts => opts.ForJob(emailReminderJobKey)				 
					.WithIdentity("EmailReminderNotificationsJob-trigger")
					.WithCronSchedule("0 * * ? * *"))
					.UsePersistentStore(s =>
					{
						s.RetryInterval = TimeSpan.FromSeconds(5);
						s.UseMicrosoftSQLite(
							options =>
							{
								options.ConnectionStringName = "Jobs";
							}
						);
						s.UseNewtonsoftJsonSerializer();
					});

			});

			services.AddQuartzHostedService(opt =>
			{
				opt.WaitForJobsToComplete = true;
			});

			return services;
		}
	}	
}
