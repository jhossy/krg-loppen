using AutoFixture;
using Krg.Database.Models;
using Krg.Domain.Models;

namespace Krg.Domain.Tests
{
    [TestClass]
	public class RegistrationTests
	{
		private readonly IFixture _fixture = new Fixture();

		[TestMethod]
		public void Ctor_CanConstruct_ProvidedInput()
		{
			//Arrange

			//Act
			Registration sut = new Registration(_fixture.Create<EventRegistration>());

			//Assert
			Assert.IsNotNull(sut);

		}
	}
}