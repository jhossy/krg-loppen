using Krg.Services.Interfaces;
using Krg.Web.Jobs;
using Microsoft.Extensions.Logging;
using AutoFixture;
using Moq;
using Krg.Domain.Models;

namespace Krg.Web.Tests.Jobs
{
	[TestClass]
    public class EmailNotificationsJobTests
    {
		private readonly IFixture _fixture = new Fixture();

        private Mock<IEmailservice> _mockEmailService = new Mock<IEmailservice>();
		private Mock<IEmailNotificationService> _mockNotificationService = new Mock<IEmailNotificationService>();
		private Mock<ILogger<EmailNotificationsJob>> _mockLogger = new Mock<ILogger<EmailNotificationsJob>>();

		private EmailNotificationsJob _sut;

		[TestInitialize]
		public void Initialize()
		{
			_fixture.Inject(_mockEmailService.Object);
			_fixture.Inject(_mockNotificationService.Object);
			_fixture.Inject(_mockLogger.Object);

			_sut = _fixture.Create<EmailNotificationsJob>();
		}

		[TestMethod]
		public async Task RunJobAsync_DoesNotTryToSendEmail_ProvidedZeroNotifications()
		{
			//Arrange
			_mockNotificationService.Setup(service => service.GetNonProcessedNotifications())
				.Returns(() => new List<Notification>());

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
				.Returns(() => _fixture.Create<List<Notification>>());

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
    }
}