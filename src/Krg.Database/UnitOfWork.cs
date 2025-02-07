
using Krg.Database.Interfaces;
using Krg.Database.Repositories;

namespace Krg.Database
{
    public class UnitOfWork : IUnitOfWork, IDisposable
	{
		public IEmailNotificationRepository EmailNotificationRepository { get; private set; }

		public IEmailReminderNotificationRepository EmailReminderNotificationRepository { get; private set; }

		public IRegistrationRepository RegistrationRepository { get; private set; }

		public IEventDateRepository EventRepository { get; private set; }

		private readonly KrgContext _context;

        public UnitOfWork(KrgContext context)
        {
			_context = context;

			EmailNotificationRepository = new EmailNotificationRepository(context);
			EmailReminderNotificationRepository = new EmailReminderNotificationRepository(context);
			RegistrationRepository = new RegistrationRepository(context);
			EventRepository = new EventDateRepository(context);
		}

        public void Commit()
		{
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
