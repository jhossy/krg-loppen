namespace Krg.Database.Interfaces
{
    public interface IUnitOfWork
    {
        IEmailNotificationRepository EmailNotificationRepository { get; }

        IEmailReminderNotificationRepository EmailReminderNotificationRepository { get; }

        IRegistrationRepository RegistrationRepository { get; }

        IEventDateRepository EventRepository { get; }

        void Commit();
    }
}
