namespace Krg.Database.Models
{
	public class Event : IUpdateableEntity
	{
		public int Id { get; set; }

		public DateTime UpdateTimeUtc { get; set; }

		public DateTime Date { get; set; }

        public required string ContactName { get; set; }

        public required string ContactEmail { get; set; }

        public required string ContactPhone { get; set; }
        
        public string? Note { get; set; }
    }
}
