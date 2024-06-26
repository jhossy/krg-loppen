﻿using Krg.Domain;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;

namespace Krg.Web.Controllers
{
    public class RegistrationsController : UmbracoAuthorizedApiController
	{
		private readonly IEventRegistrationService _eventRegistrationService;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IExcelService _excelService;
        public RegistrationsController(
			IEventRegistrationService eventRegistrationService,
			IEmailNotificationService emailNotificationService,
			IExcelService excelService)
        {
            _eventRegistrationService = eventRegistrationService;
			_emailNotificationService = emailNotificationService;
			_excelService = excelService;
        }

		[HttpGet]
		public List<BackofficeRegistrationDto> GetRegistrations(int year)
		{
			int parsedYear = year == 0 ? DateTime.Now.Year : year;

			return _eventRegistrationService
				.GetNonDeletedRegistrations(parsedYear)
				.OrderBy(reg => reg.EventDate)
				.ThenBy(reg => reg.UpdateTimeUtc)
				.Select(reg => new BackofficeRegistrationDto(reg))
				.ToList();
		}

		internal List<BackofficeRegistrationDto> GetAllRegistrations(int year)
		{
			int parsedYear = year == 0 ? DateTime.Now.Year : year;

			return _eventRegistrationService
				.GetAllRegistrations(parsedYear)
                .OrderBy(reg => reg.EventDate)
                .ThenBy(reg => reg.UpdateTimeUtc)
                .Select(reg => new BackofficeRegistrationDto(reg))
				.ToList();
		}

		[HttpGet]
		public IActionResult ExportAsExcel(int year)
		{
			List<BackofficeRegistrationDto> registrations = GetAllRegistrations(year);

			string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			
			string fileName = $"Export_{DateTime.Now.ToDkExportDate()}.xlsx";

			byte[] excelData = _excelService.CreateExcel(year, registrations);

			return File(excelData, contentType, fileName);
		}

		[HttpPost]
		public IActionResult Delete(int id, int year)
		{
			var registration = _eventRegistrationService.GetById(id);

			if (registration == null) return Ok();

			_eventRegistrationService.RemoveRegistration(id);

			_emailNotificationService.CancelReminder(id);

			return Ok(GetRegistrations(year));
		}
	}
}
