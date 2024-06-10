namespace Krg.Database.Models
{
	public class EmailReminderNotification : IUpdateableEntity
	{
		public int Id { get; set; }

		public int UmbracoEventNodeId { get; set; }

		public DateTime UpdateTimeUtc { get; set; }

		public DateTime EventDate { get; set; }

		public required string From { get; set; }

		public required string To { get; set; }

		public required string Subject { get; set; }

		public required string Body { get; set; }

		public bool Processed { get; set; }

		public bool IsCancelled { get; set; }
	}
}
