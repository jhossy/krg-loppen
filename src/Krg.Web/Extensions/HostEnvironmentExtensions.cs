namespace Krg.Web.Extensions
{
	public static class HostEnvironmentExtensions
	{
		private const string Azure = "Azure";

		public static bool IsAzure(this IHostEnvironment hostEnvironment)
		{
			return hostEnvironment.EnvironmentName.InvariantEquals(Azure);
		}
	}
}
