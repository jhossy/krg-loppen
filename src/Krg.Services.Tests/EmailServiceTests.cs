﻿using AutoFixture;
using Krg.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Mail;

namespace Krg.Services.Tests
{
    [TestClass]
	public class EmailServiceTests
	{
		private readonly IFixture _fixture = new Fixture();		
		private IEmailservice _sut = null!;

		[TestInitialize]
		public void Initialize()
		{
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

			string mailDir = "c:\\temp\\mails";
			if (!Directory.Exists(mailDir))
			{
				Directory.CreateDirectory(mailDir);
			}
			smtpClient.PickupDirectoryLocation = mailDir;

			_fixture.Inject(smtpClient);

			var loggerMock = new Mock<ILogger<EmailService>>();
			_fixture.Inject(loggerMock.Object);
			_sut = _fixture.Create<EmailService>();
		}

		[TestMethod]
		[TestCategory("IntegrationTests")]
		public async Task SendEmail_ActuallySendsEmail_ProvidedValidInput()
		{
			//Arrange
			
			//Act

			//Assert
			await _sut.SendEmail(
				"sender@test.dk", 
				new[] { "receiver@test.dk" } , 
				"Tak for din tilmelding til loppekørsel søndag d. 17. december 2023 kl. 09:00",
				$"Kære Sigfred.<br><br>Du har tilmeldt dig Loppekørsel søndag d. 17. december 2023 kl. 09:00.<br>" +
				$"Tak for din deltagelse, som er med til at sikre vores børn nogle gode spejderoplevelser!<br><br>" +
				$"Vi mødes på Hundested Genbrugsstation, Håndværkervej 16, 3390 Hundested, og forventer at være færdige efter ca. 2 timer.<br>" +
				$"Din kontaktperson på dagen er:<br>Jette Simonsen<br>Telefon: 24448429<br>E-mail: simonsenjette@yahoo.dk<br><br>" +
				$"Hvis du bliver forhindret i at deltage er der vigtigt at du kontakter din kontaktperson hurtigst muligt. " +
				$"Din kontaktperson vil også kunne hjælpe dig hvis du har praktiske spørgsmål.<br><br>Venlig hilsen<br>Knud Rasmussengruppen"
			);
		}

		//[TestMethod]
		//[TestCategory("IntegrationTests")]
		//public async Task SendEmail_Throws_ProvidedExceptionOccurs()
		//{
		//	//Arrange

		//	//Act

		//	//Assert
		//	await Assert.ThrowsExceptionAsync<Exception>(() => 
		//		_sut.SendEmail("test@mail.dk",
		//			new[] { "test@mail.dk" } ,
		//			"test@mail.dk", 
		//			"email body"));
			
		//}
	}
}
