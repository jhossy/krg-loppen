using Krg.Database.Interfaces;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Krg.Services
{

    public class EmailNotificationService : IEmailNotificationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<IEmailNotificationService> _logger;

		public EmailNotificationService(
			IUnitOfWork unitOfWork,
			ILogger<EmailNotificationService> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public void AddNotification(AddRegistrationRequest registrationRequest, string emailSender)
		{
			if (registrationRequest == null || string.IsNullOrEmpty(registrationRequest.Email)) return;

			string contactName = GetContactNameOrFallback(registrationRequest);
			string contactPhone = GetContactPhoneOrFallback(registrationRequest);
			string contactEmail = GetContactEmailOrFallback(registrationRequest);

			_unitOfWork.EmailNotificationRepository.AddNotification(new EmailNotification
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

			_unitOfWork.Commit();
		}

		public void AddReminder(AddRegistrationRequest registrationRequest, string emailSender, int eventRegistrationId)
		{
			if (registrationRequest == null || string.IsNullOrEmpty(registrationRequest.Email)) return;

			string contactName = GetContactNameOrFallback(registrationRequest);
			string contactPhone = GetContactPhoneOrFallback(registrationRequest);
			string contactEmail = GetContactEmailOrFallback(registrationRequest);

			_unitOfWork.EmailReminderNotificationRepository.AddReminder(new EmailReminderNotification
			{
				EventDate = registrationRequest.EventDate,
				From = emailSender,
				To = registrationRequest.Email,
				Subject = $"Påmindelse om loppekørsel søndag d. {registrationRequest.EventDate.ToString("d. MMMM", CultureInfo.CreateSpecificCulture("da-DK"))} kl. 09:00",
				Body = $"Kære {registrationRequest.Name}.<br><br>Husk at du har tilmeldt dig Loppekørsel søndag d. {registrationRequest.EventDate.ToString("d. MMMM", CultureInfo.CreateSpecificCulture("da-DK"))} kl. 09:00.<br>" +
					$"Vi mødes på Hundested Genbrugsstation, Håndværkervej 16, 3390 Hundested, og forventer at være færdige efter ca. 2 timer.<br>" +
					$"Din kontaktperson på dagen er:<br>{contactName}<br>Telefon: {contactPhone}<br>E-mail: {contactEmail}<br><br>" +
					$"Hvis du bliver forhindret i at deltage er der vigtigt at du kontakter din kontaktperson hurtigst muligt. " +
					$"Din kontaktperson vil også kunne hjælpe dig hvis du har praktiske spørgsmål.<br><br>Venlig hilsen<br>Knud Rasmussengruppen",
				Processed = false,
				UpdateTimeUtc = DateTime.UtcNow,
				//UmbracoEventNodeId = registrationRequest.UmbracoNodeId,
				UmbracoEventNodeId = 0, //TODO remove
				FkEventRegistrationId = eventRegistrationId
			});

			_unitOfWork.Commit();
		}

		public void CancelReminder(int eventRegistrationId)
		{
			try
			{
				_unitOfWork.EmailReminderNotificationRepository.CancelReminder(eventRegistrationId);

				_unitOfWork.Commit();
			}
			catch (Exception ex)
			{
				_logger.LogError($"RemoveNotification error: {ex.Message}", ex);
			}
		}

		public List<Notification> GetNonProcessedNotifications()
		{
			try
			{
				return _unitOfWork.EmailNotificationRepository
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

		public List<EmailReminder> GetNonProcessedReminders()
		{
			try
			{
				return _unitOfWork.EmailReminderNotificationRepository
					.GetUnprocessedReminders()
					.Select(notification => new EmailReminder(notification))
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetNonProcessedReminders error: {ex.Message}", ex);
			}
			return new List<EmailReminder>();
		}

		public void RemoveNotification(int id)
		{
			try
			{
				_unitOfWork.EmailNotificationRepository.RemoveNotification(id);

				_unitOfWork.Commit();
			}
			catch(Exception ex)
			{
				_logger.LogError($"RemoveNotification error: {ex.Message}", ex);
			}			
		}

		public void RemoveReminder(int id)
		{
			try
			{
				_unitOfWork.EmailReminderNotificationRepository.SetIsProcessed(id);

				_unitOfWork.Commit();
			}
			catch (Exception ex)
			{
				_logger.LogError($"RemoveReminder error: {ex.Message}", ex);
			}
		}

		internal string GetContactNameOrFallback(AddRegistrationRequest registrationRequest)
		{
			return string.IsNullOrEmpty(registrationRequest.ContactName) ? Domain.Constants.FallBackContactName : registrationRequest.ContactName;
		}

		internal string GetContactPhoneOrFallback(AddRegistrationRequest registrationRequest)
		{
			return string.IsNullOrEmpty(registrationRequest.ContactPhone) ? Domain.Constants.FallBackContactPhoneNo : registrationRequest.ContactPhone;
		}

		internal string GetContactEmailOrFallback(AddRegistrationRequest registrationRequest)
		{
			return string.IsNullOrEmpty(registrationRequest.ContactEmail) ? Domain.Constants.FallBackContactEmail : registrationRequest.ContactEmail;
		}
	}
}
