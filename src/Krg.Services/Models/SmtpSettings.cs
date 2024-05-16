namespace Krg.Services
{
	public class SmtpSettings
	{
        public required string Host { get; set; }

		public int Port { get; set; }

        public required string UserName { get; set; }

		public required string Password { get; set; }

		public bool UseLocalShare()
		{
			return string.IsNullOrEmpty(Host) || 
				Port == 0 || 
				string.IsNullOrEmpty(UserName) || 
				string.IsNullOrEmpty(Password);
		}
	}
}
