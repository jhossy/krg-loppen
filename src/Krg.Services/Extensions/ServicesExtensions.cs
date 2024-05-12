using Krg.Database;
using Krg.Services.Interfaces;
using Krg.Web.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Numerics;

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
			services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();

			services.AddTransient<IEmailservice>(provider =>
			{
				ILogger<EmailService> logger = provider.GetService<ILogger<EmailService>>();

				SmtpClient smtpClient = new SmtpClient();
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

				string mailDir = "c:\\temp\\mails";
				if (!Directory.Exists(mailDir))
				{
					Directory.CreateDirectory(mailDir);
				}
				smtpClient.PickupDirectoryLocation = mailDir;

				return new EmailService(logger, smtpClient);
			});

			return services;
		}
	}
}
