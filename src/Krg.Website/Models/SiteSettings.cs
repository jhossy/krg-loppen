namespace Krg.Website.Models
{
	public class SiteSettings
	{
        public required string EmailFromAddress { get; set; }

		public required int[] YearsToShow { get; set; }
    }
}
