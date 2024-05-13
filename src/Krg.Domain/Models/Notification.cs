using Krg.Database.Models;
using System.Diagnostics.CodeAnalysis;

namespace Krg.Domain.Models
{
	public class Notification
	{
		[SetsRequiredMembers]
		public Notification(EmailNotification emailNotification)
        {
			Id = emailNotification.Id;
			EventDate = emailNotification.EventDate;
            From = emailNotification.From;
			To = emailNotification.To;
			Subject = emailNotification.Subject;
			Body = emailNotification.Body;
			Processed = emailNotification.Processed;
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
