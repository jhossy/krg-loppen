using Krg.Services.Interfaces;
using Krg.Web.Jobs;
using Microsoft.Extensions.Logging;
using AutoFixture;
using Moq;
using Krg.Domain.Models;

namespace Krg.Web.Tests.Jobs
{
	[TestClass]
    public class EmailReminderNotificationsJobTests
	{
		private readonly IFixture _fixture = new Fixture();

        private Mock<IEmailservice> _mockEmailService = new Mock<IEmailservice>();
		private Mock<IEmailNotificationService> _mockNotificationService = new Mock<IEmailNotificationService>();
		private Mock<ILogger<EmailReminderNotificationsJob>> _mockLogger = new Mock<ILogger<EmailReminderNotificationsJob>>();

		private EmailReminderNotificationsJob _sut;

		[TestInitialize]
		public void Initialize()
		{
			_fixture.Inject(_mockEmailService.Object);
			_fixture.Inject(_mockNotificationService.Object);
			_fixture.Inject(_mockLogger.Object);

			_sut = _fixture.Create<EmailReminderNotificationsJob>();
		}

		[TestMethod]
		public async Task RunJobAsync_DoesNotTryToSendEmail_ProvidedZeroNotifications()
		{
			//Arrange
			_mockNotificationService.Setup(service => service.GetNonProcessedNotifications())
				.Returns(() => new List<Notification>());

			_mockNotificationService.Setup(service => service.GetNonProcessedReminders())
				.Returns(() => new List<EmailReminder>());

			//Act
			await _sut.RunJobAsync();

			//Assert
			_mockEmailService.Verify(service => service.SendEmail(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[TestMethod]
        public async Task RunJobAsync_RetriesThreeTimes_ProvidedErrorOccurs()
        {
			//Arrange
			_mockNotificationService.Setup(service => service.GetNonProcessedNotifications())
				.Returns(() => new List<Notification>());

			_mockNotificationService.Setup(service => service.GetNonProcessedReminders())
				.Returns(() => _fixture.Create<List<EmailReminder>>());

			int timesCalled = 0;
			_mockEmailService.Setup(emailService => emailService.SendEmail(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback(() => timesCalled++)
				.Throws<Exception>();

			//Act
			try 
			{
				await _sut.RunJobAsync();
			}
			catch(Exception ex)
			{

			}
			

			//Assert
			Assert.AreEqual(timesCalled, 3);
        }

		[TestMethod]
		public async Task RunJobAsync_DoesNotSendEmail_ProvidedPendingRegularNotifications()
		{
			//Arrange
			_mockNotificationService.Setup(service => service.GetNonProcessedNotifications())
				.Returns(() => _fixture.Create<List<Notification>>());

			_mockNotificationService.Setup(service => service.GetNonProcessedReminders())
				.Returns(() => _fixture.Create<List<EmailReminder>>());

			int timesCalled = 0;
			//_mockEmailService.Setup(emailService => emailService.SendEmail(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()));

			//Act
			await _sut.RunJobAsync();

			//Assert
			_mockEmailService.Verify(service => service.SendEmail(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}
	}
}