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
		private Mock<INotificationRepository> _mockNotificationRepository = new Mock<INotificationRepository>();
		private Mock<ILogger<EmailNotificationService>> _logger = new Mock<ILogger<EmailNotificationService>>();
		private IEmailNotificationService _sut = null!;

		[TestInitialize]
		public void Initialize()
		{
			_fixture.Inject(_mockNotificationRepository.Object);
			_fixture.Inject(_logger.Object);
			_sut = _fixture.Create<EmailNotificationService>();
		}

		[TestMethod]
		public void AddRegistration_DoesNotCallRepository_ProvidedInvalidInput()
		{
			//Arrange

			//Act
			_sut.AddNotification(null);

			//Assert
			_mockNotificationRepository.Verify(mock => mock.AddNotification(It.IsAny<EmailNotification>()), Times.Never());
		}

		[TestMethod]
		public void AddRegistration_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.AddNotification(_fixture.Create<AddRegistrationRequest>());

			//Assert
			_mockNotificationRepository.Verify(mock => mock.AddNotification(It.IsAny<EmailNotification>()), Times.Once());
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
		}
	}
}