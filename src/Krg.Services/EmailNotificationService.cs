using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

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

		public void AddNotification(AddRegistrationRequest registrationRequest, string emailSender)
		{
			if (registrationRequest == null || string.IsNullOrEmpty(registrationRequest.Email)) return;

			string contactName = string.IsNullOrEmpty(registrationRequest.ContactName) ? Krg.Domain.Constants.FallBackContactName : registrationRequest.ContactName;
			string contactPhone = string.IsNullOrEmpty(registrationRequest.ContactPhone) ? Krg.Domain.Constants.FallBackContactPhoneNo : registrationRequest.ContactPhone; ;
			string contactEmail = string.IsNullOrEmpty(registrationRequest.ContactEmail) ? Krg.Domain.Constants.FallBackContactEmail : registrationRequest.ContactEmail; ;

			_notificationRepository.AddNotification(new EmailNotification
			{
				EventDate = registrationRequest.EventDate,
				From = emailSender,
				To = registrationRequest.Email,
				Subject = $"Tak for din tilmelding til loppekørsel søndag d. {registrationRequest.EventDate.ToString("d. MMMM", CultureInfo.CreateSpecificCulture("da-DK"))} kl. 09:00",
				Body = $"Kære {registrationRequest.Name}.<br><br>Du har tilmeldt dig Loppekørsel søndag d. {registrationRequest.EventDate.ToString("d. MMMM", CultureInfo.CreateSpecificCulture("da-DK"))} kl. 09:00.<br>" +
					$"Tak for din deltagelse, som er med til at sikre vores børn nogle gode spejderoplevelser!<br><br>" +
					$"Vi mødes på Hundested Genbrugsstation, Håndværkervej 16, 3390 Hundested, og forventer at være færdige efter ca. 2 timer.<br>" +
					$"Din kontaktperson på dagen er:<br>{contactName}<br>Telefon: {contactPhone}<br>E-mail: {contactEmail}<br><br>" +
					$"Hvis du bliver forhindret i at deltage er der vigtigt at du kontakter din kontaktperson hurtigst muligt. " +
					$"Din kontaktperson vil også kunne hjælpe dig hvis du har praktiske spørgsmål.<br><br>Venlig hilsen<br>Knud Rasmussengruppen",
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

		public void RemoveNotification(int id)
		{
			try
			{
				_notificationRepository.RemoveNotification(id);
			}
			catch(Exception ex)
			{
				_logger.LogError($"RemoveNotification error: {ex.Message}", ex);
			}			
		}
	}
}
