using System.Globalization;

namespace Krg.Web.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToDkDate(this DateTime dateTime)
		{
			return dateTime.ToString("dd-MM-yyyy", new CultureInfo("da-DK"));
		}

		public static string ToDkExportDate(this DateTime dateTime)
		{
			return dateTime.ToString("dd-MM-yyyy-HH-mm-ss", new CultureInfo("da-DK"));
		}
	}
}
