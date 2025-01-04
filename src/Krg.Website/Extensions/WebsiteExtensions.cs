using Krg.Database;
using Krg.Website.Jobs;
using Krg.Website.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Quartz;

namespace Krg.Website.Extensions
{
	public static class WebsiteExtensions
	{
		public static IServiceCollection AddWebsiteExtensions(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddControllersWithViews()
				.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter()));

            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

			services.AddAuthorization();
			services.AddAuthentication()
				.AddCookie(IdentityConstants.ApplicationScheme, options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
					options.SlidingExpiration = true;
					options.AccessDeniedPath = "/Account/AccessDenied";
				});
				
			services.AddIdentityCore<IdentityUser>(options =>
				{
					// options.SignIn.RequireConfirmedAccount = true;
				})
			 	.AddEntityFrameworkStores<ApplicationDbContext>()
			    .AddApiEndpoints(); //TODO remove
			
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
					.WithCronSchedule("0 2/5 0 ? * * *")) //every 5 minutes, starting at minute 02
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

				var emailReminderJobKey = new JobKey(nameof(EmailReminderNotificationsJob));

				q.AddJob<EmailReminderNotificationsJob>(emailReminderJobKey)
				 .AddTrigger(opts => opts.ForJob(emailReminderJobKey)				 
					.WithIdentity("EmailReminderNotificationsJob-trigger")
					.WithCronSchedule("0 0 2/6 ? * * *")) //every 6 hrs, starting at hour 02
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
