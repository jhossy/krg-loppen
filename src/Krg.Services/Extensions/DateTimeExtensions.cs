using System.Globalization;

namespace Krg.Web.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToDkDate(this DateTime dateTime)
		{
			return dateTime.ToString("dd-MM-yyyy", new CultureInfo("da-DK"));
		}

		public static string ToDkMonth(this DateTime dateTime)
		{			
			string month = dateTime.ToString("MMMM", new CultureInfo("da-DK"));

			return month[0].ToString().ToUpper() + month.Substring(1);
		}

		public static string ToDkExportDate(this DateTime dateTime)
		{
			return dateTime.ToString("dd-MM-yyyy-HH-mm-ss", new CultureInfo("da-DK"));
		}
	}
}
