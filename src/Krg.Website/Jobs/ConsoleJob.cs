using Quartz;

namespace Krg.Website.Jobs
{
	[DisallowConcurrentExecution]

	public class ConsoleJob : IJob
	{
		public Task Execute(IJobExecutionContext context)
		{
			Console.WriteLine($"Job is starting...{DateTime.Now}");

			Thread.Sleep( 1000 );

			Console.WriteLine("Job ended...");

			return Task.CompletedTask;
		}
	}
}
