using AutoFixture;
using Krg.Database;
using Krg.Database.Models;
using Krg.Domain.Models;
using Krg.Services.Interfaces;
using Moq;

namespace Krg.Services.Tests
{
    [TestClass]
	public class EventRegistrationServiceTests
	{
		private readonly IFixture _fixture = new Fixture();
		private Mock<IRegistrationRepository> _mockRegistrationRepository = new Mock<IRegistrationRepository>();
		private IEventRegistrationService _sut = null!;

		[TestInitialize]
		public void Initialize()
		{
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
		public void AddRegistration_CallsRepository_ProvidedValidInput()
		{
			//Arrange

			//Act
			_sut.AddRegistration(_fixture.Create<int>(), _fixture.Create<AddRegistrationRequest>());

			//Assert
			_mockRegistrationRepository.Verify(mock => mock.AddRegistration(It.IsAny<EventRegistration>()), Times.Once());
		}

		[TestMethod]
		public void GetNonDeletedRegistrations_GetsOnlyActiveRegistrations_ProvidedTheyExist()
		{
			//Arrange
			var registrations = _fixture.Build<EventRegistration>()
										.With(p => p.IsCancelled, false)
										.CreateMany();

			_mockRegistrationRepository.Setup(repository => repository.GetNonDeletedRegistrations(It.IsAny<int>()))
				.Returns(() => registrations.ToList());

			//Act
			var result = _sut.GetNonDeletedRegistrations(_fixture.Create<int>());

			//Assert
			Assert.IsTrue(result.All(p => !p.IsCancelled));
		}

		[TestMethod]
		public void GetAllRegistrations_GetsAllRegistrations_ProvidedTheyExist()
		{
			//Arrange
			var registrations = _fixture.Build<EventRegistration>()
										.CreateMany();

			_mockRegistrationRepository.Setup(repository => repository.GetAllRegistrations(It.IsAny<int>()))
				.Returns(() => registrations.ToList());

			//Act
			var result = _sut.GetAllRegistrations(_fixture.Create<int>());

			//Assert
			Assert.IsTrue(result.Count == registrations.Count());
			Assert.IsTrue(registrations.All(reg => result.Any(res => reg.Id == res.Id)));
		}
	}
}