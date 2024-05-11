using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        void AddNotification(AddRegistrationRequest registrationRequest);

        List<Notification> GetNonProcessedNotifications();
    }
}
