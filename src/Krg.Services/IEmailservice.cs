namespace Krg.Services
{
	public interface IEmailservice
	{
		Task SendEmail(string sender, string[] receivers, string subject, string body);
	}
}
