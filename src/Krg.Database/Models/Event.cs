namespace Krg.Database.Models
{
	public class Event : IUpdateableEntity
	{
		public int Id { get; set; }

		public DateTime UpdateTimeUtc { get; set; }

		public DateTime Date { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }
    }
}
