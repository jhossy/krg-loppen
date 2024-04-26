using AutoFixture;
using Krg.Database;
using Krg.Database.Models;
using Krg.Domain;
using Moq;

namespace Krg.Services.Tests
{
	[TestClass]
	public class EventRegistrationServiceTests
	{
		private readonly IFixture _fixture = new Fixture();
		private Mock<IRegistrationRepository> _mockRegistrationRepository;
		private IEventRegistrationService _sut;

		[TestInitialize]
		public void Initialize()
		{
			_mockRegistrationRepository = new Mock<IRegistrationRepository>();
			_fixture.Inject(_mockRegistrationRepository.Object);
			_sut = _fixture.Create<EventRegistrationService>();
		}

		[TestMethod]
		public void AddRegistration_DoesNotCallRepository_ProvidedInvalidInput()
		{
			//Arrange

			//Act
			_sut.AddRegistration(_fixture.Create<int>(), null);

			//Assert
			_mockRegistrationRepository.Verify(mock => mock.AddRegistration(It.IsAny<EventRegistration>()), Times.Never());
		}

		[TestMethod]
		public void AddRegistration_DoesNotCallRepository_ProvidedMissingDepartment()
		{
			//Arrange
			var registrationRequest = _fixture.Create<AddRegistrationRequest>();
			registrationRequest.Department = null;

			//Act
			_sut.AddRegistration(_fixture.Create<int>(), registrationRequest);

			//Assert
			_mockRegistrationRepository.Verify(mock => mock.AddRegistration(It.IsAny<EventRegistration>()), Times.Never());
		}

		[TestMethod]
		public void AddRegistration_DoesNotCallRepository_ProvidedMissingPhoneNo()
		{
			//Arrange
			var registrationRequest = _fixture.Create<AddRegistrationRequest>();
			registrationRequest.PhoneNo = null;

			//Act
			_sut.AddRegistration(_fixture.Create<int>(), registrationRequest);

			//Assert
			_mockRegistrationRepository.Verify(mock => mock.AddRegistration(It.IsAny<EventRegistration>()), Times.Never());
		}

		[TestMethod]
		public void AddRegistration_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.AddRegistration(_fixture.Create<int>(), _fixture.Create<AddRegistrationRequest>());

			//Assert
			_mockRegistrationRepository.Verify(mock => mock.AddRegistration(It.IsAny<EventRegistration>()), Times.Once());
		}
	}
}