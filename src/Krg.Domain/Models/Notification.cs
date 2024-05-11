using Krg.Database.Models;
using System.Diagnostics.CodeAnalysis;

namespace Krg.Domain.Models
{
	public class Notification
	{
		//[SetsRequiredMembers]
		//public Notification(string from, string to, string subject, string body)
  //      {
  //          From = from;
		//	To = to;
		//	Subject = subject;
		//	Body = body;
  //      }

		[SetsRequiredMembers]
		public Notification(EmailNotification emailNotification)
        {
            From = emailNotification.From;
			To = emailNotification.To;
			Subject = emailNotification.Subject;
			Body = emailNotification.Body;
			Processed = emailNotification.Processed;
        }

        public required string From { get; set; }

		public required string To { get; set; }

		public required string Subject { get; set; }

		public required string Body { get; set; }

		public bool Processed { get; set; }
	}
}
