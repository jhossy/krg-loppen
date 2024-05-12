using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krg.Services
{

	public class EmailNotificationService : IEmailNotificationService
	{
		private readonly IEmailNotificationRepository _notificationRepository;
		private readonly ILogger<IEmailNotificationService> _logger;

		public EmailNotificationService(
			IEmailNotificationRepository notificationRepository,
			ILogger<EmailNotificationService> logger)
		{
			_notificationRepository = notificationRepository;
			_logger = logger;
		}

		public void AddNotification(AddRegistrationRequest registrationRequest)
		{
			if (registrationRequest == null || string.IsNullOrEmpty(registrationRequest.Email)) return;

			_notificationRepository.AddNotification(new EmailNotification
			{
				EventDate = registrationRequest.EventDate,
				From = "loppen@spejderknud.dk",
				To = registrationRequest.Email,
				Subject = "Tilmelding til loppekørsel",
				Body = "email content here",
				Processed = false,
				UpdateTimeUtc = DateTime.UtcNow
			});
		}

		public List<Notification> GetNonProcessedNotifications()
		{
			try
			{
				return _notificationRepository
					.GetUnprocessedNotifications()
					.Select(notification => new Notification(notification))
					.ToList();
			}
			catch(Exception ex)
			{
				_logger.LogError($"GetNonProcessedNotifications error: {ex.Message}", ex);
			}
			return new List<Notification>();
		}
	}
}
