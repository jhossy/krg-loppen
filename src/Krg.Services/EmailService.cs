using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace Krg.Services
{
    public class EmailService : IEmailservice
	{
		private ILogger<EmailService> _logger;
		private readonly SmtpClient _smtpClient;

		public EmailService(ILogger<EmailService> logger, SmtpClient smtpClient) 
		{
			_logger = logger;
			_smtpClient = smtpClient;
		}

		public async Task SendEmail(string sender, string[] receivers, string subject, string body)
		{
			MailMessage message = new MailMessage
			{
				Body = body,
				Subject = subject,
				From = new MailAddress(sender),
				IsBodyHtml = true
			};

			foreach(string to in receivers)
			{
				message.To.Add(new MailAddress(to));
			}
			
			try
			{
				_logger.LogDebug("Sending email: {sender}, {receiver}, {subject}", sender, string.Join(",", receivers), subject);

				await _smtpClient.SendMailAsync(message);
			} 
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}
	}
}
