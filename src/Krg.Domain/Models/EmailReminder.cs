using Krg.Database.Models;
using System.Diagnostics.CodeAnalysis;

namespace Krg.Domain.Models
{
	public class EmailReminder
	{
		[SetsRequiredMembers]
		public EmailReminder(EmailReminderNotification emailReminderNotification)
		{
			Id = emailReminderNotification.Id;
			EventDate = emailReminderNotification.EventDate;
			From = emailReminderNotification.From;
			To = emailReminderNotification.To;
			Subject = emailReminderNotification.Subject;
			Body = emailReminderNotification.Body;
			Processed = emailReminderNotification.Processed;
		}

		public int Id { get; set; }

		public DateTime EventDate { get; set; }

		public required string From { get; set; }

		public required string To { get; set; }

		public required string Subject { get; set; }

		public required string Body { get; set; }

		public bool Processed { get; set; }
	}
}
