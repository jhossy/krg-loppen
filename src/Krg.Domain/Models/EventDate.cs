using System.Text.Json.Serialization;

namespace Krg.Domain.Models
{
	public class EventDate
	{
		public int Id { get; set; }
		
		public DateTime Date { get; set; }

		public required string ContactName { get; set; }

		public required string ContactEmail { get; set; }

		public required string ContactPhone { get; set; }
	}
}
