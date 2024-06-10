using AutoFixture;
using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Krg.Services.Tests
{
    [TestClass]
	public class EmailNotificationServiceTests
	{
		private readonly IFixture _fixture = new Fixture();
		private Mock<IEmailNotificationRepository> _mockNotificationRepository = new Mock<IEmailNotificationRepository>();
		private Mock<IEmailReminderNotificationRepository> _mockEmailReminderNotificationRepository = new Mock<IEmailReminderNotificationRepository>();
		private Mock<ILogger<EmailNotificationService>> _logger = new Mock<ILogger<EmailNotificationService>>();
		private IEmailNotificationService _sut = null!;

		[TestInitialize]
		public void Initialize()
		{
			_fixture.Inject(_mockNotificationRepository.Object);
			_fixture.Inject(_mockEmailReminderNotificationRepository.Object);
			_fixture.Inject(_logger.Object);
			_sut = _fixture.Create<EmailNotificationService>();
		}

		[TestMethod]
		public void AddNotification_DoesNotCallRepository_ProvidedInvalidInput()
		{
			//Arrange

			//Act
			_sut.AddNotification(null, null);

			//Assert
			_mockNotificationRepository.Verify(mock => mock.AddNotification(It.IsAny<EmailNotification>()), Times.Never());
		}

		[TestMethod]
		public void AddNotification_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.AddNotification(_fixture.Create<AddRegistrationRequest>(), _fixture.Create<string>());

			//Assert
			_mockNotificationRepository.Verify(mock => mock.AddNotification(It.IsAny<EmailNotification>()), Times.Once());
		}

		[TestMethod]
		public void AddReminder_DoesNotCallRepository_ProvidedInvalidInput()
		{
			//Arrange

			//Act
			_sut.AddReminder(null, null, 0);

			//Assert
			_mockEmailReminderNotificationRepository.Verify(mock => mock.AddReminder(It.IsAny<EmailReminderNotification>()), Times.Never());
		}

		[TestMethod]
		public void AddReminder_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.AddReminder(_fixture.Create<AddRegistrationRequest>(), _fixture.Create<string>(), _fixture.Create<int>());

			//Assert
			_mockEmailReminderNotificationRepository.Verify(mock => mock.AddReminder(It.IsAny<EmailReminderNotification>()), Times.Once());
		}

		[TestMethod]
		public void RemoveNotification_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.RemoveNotification(_fixture.Create<int>());

			//Assert
			_mockNotificationRepository.Verify(mock => mock.RemoveNotification(It.IsAny<int>()), Times.Once());
		}

		[TestMethod]
		public void CancelReminder_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.CancelReminder(_fixture.Create<int>());

			//Assert
			_mockEmailReminderNotificationRepository.Verify(mock => mock.CancelReminder(It.IsAny<int>()), Times.Once());
		}


		[TestMethod]
		public void GetNonProcessedNotifications_GetsOnlyActiveNotifications_ProvidedTheyExist()
		{
			//Arrange
			var emailNotifications = _fixture.Build<EmailNotification>()
										.With(p => p.Processed, false)
										.CreateMany();

			_mockNotificationRepository.Setup(repository => repository.GetUnprocessedNotifications())
				.Returns(() => emailNotifications.ToList());

			//Act
			var result = _sut.GetNonProcessedNotifications();

			//Assert
			Assert.IsTrue(result.All(p => !p.Processed));
			Assert.IsTrue(emailNotifications.Count() == result.Count);
		}

		[TestMethod]
		public void GetNonProcessedReminders_GetsOnlyActiveNotifications_ProvidedTheyExist()
		{
			//Arrange
			var emailReminderNotifications = _fixture.Build<EmailReminderNotification>()
										.With(p => p.Processed, false)
										.CreateMany();

			_mockEmailReminderNotificationRepository.Setup(repository => repository.GetUnprocessedReminders())
				.Returns(() => emailReminderNotifications.ToList());

			//Act
			var result = _sut.GetNonProcessedReminders();

			//Assert
			Assert.IsTrue(result.All(p => !p.Processed));
			Assert.IsTrue(emailReminderNotifications.Count() == result.Count);
		}
	}
}