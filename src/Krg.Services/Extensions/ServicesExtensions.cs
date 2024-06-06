using Krg.Database;
using Krg.Services.Interfaces;
using Krg.Web.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Krg.Services.Extensions
{
	public static class ServicesExtensions
	{
		public static IServiceCollection AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IEventRegistrationService, EventRegistrationService>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IExcelService, ExcelService>();
			services.AddTransient<IEmailNotificationService, EmailNotificationService>();
			services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
			services.AddTransient<IEmailReminderNotificationRepository, EmailReminderNotificationRepository>();

			services.AddTransient<IEmailservice>(provider =>
			{
				ILogger<EmailService> logger = provider.GetRequiredService<ILogger<EmailService>>();

				SmtpSettings settings = configuration
											.GetRequiredSection("SmtpSettings")
											.Get<SmtpSettings>();
				SmtpClient smtpClient;

				if (settings.UseLocalShare())
				{
					smtpClient = new SmtpClient();
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

					string mailDir = AppContext.BaseDirectory + "/mails";
					if (!Directory.Exists(mailDir))
					{
						Directory.CreateDirectory(mailDir);
					}
					smtpClient.PickupDirectoryLocation = mailDir;
				}
				else
				{
					smtpClient = new SmtpClient(settings.Host, settings.Port);
					smtpClient.Credentials = new NetworkCredential(settings.UserName, settings.Password);
				}				

				return new EmailService(logger, smtpClient);
			});

			return services;
		}
	}
}
