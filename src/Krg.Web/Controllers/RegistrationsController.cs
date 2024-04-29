using Krg.Domain;
using Krg.Services;
using Krg.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Controllers;

namespace Krg.Web.Controllers
{
	public class RegistrationsController : UmbracoApiController /*UmbracoAuthorizedJsonController*/
	{
		private readonly IEventRegistrationService _eventRegistrationService;
		private readonly IExcelService _excelService;
        public RegistrationsController(
			IEventRegistrationService eventRegistrationService,
			IExcelService excelService)
        {
            _eventRegistrationService = eventRegistrationService;
			_excelService = excelService;
        }

		private IEnumerable<BackofficeRegistrationDto> GetRegistrations(int year)
		{
			int parsedYear = year == 0 ? DateTime.Now.Year : year;

			return _eventRegistrationService
				.GetRegistrations()
				.Where(x => x.EventDate.Year == parsedYear)
				.OrderBy(reg => reg.EventDate)
				.Select(reg => new BackofficeRegistrationDto(reg))
				.ToList();
		}

		//https://localhost:44396/umbraco/api/registrations/exportasexcel?year=2024
		[HttpGet]
		public IActionResult ExportAsExcel(int year)
		{
			var registrations = GetRegistrations(year);

			using (var package = new ExcelPackage(new FileInfo($"Export_{DateTime.Now.ToDkDate()}.xlsx")))
			{
				var worksheet = package.Workbook.Worksheets.Add(year.ToString());

				//Add the headers
				worksheet.Cells[1, 1].Value = "Dato";
				worksheet.Cells[1, 2].Value = "Navn";
				worksheet.Cells[1, 3].Value = "Email";
				worksheet.Cells[1, 4].Value = "Afdeling";
				worksheet.Cells[1, 5].Value = "Antal voksne";
				worksheet.Cells[1, 6].Value = "Antal børn";
				worksheet.Cells[1, 7].Value = "Medbringer trailer";

				//Add some items...
				int i = 2;
				foreach (var registration in registrations)
				{
					worksheet.Cells["A" + i].Value = registration.EventDate;
					worksheet.Cells["B" + i].Value = registration.Name;
					worksheet.Cells["C" + i].Value = registration.Email;
					worksheet.Cells["D" + i].Value = registration.Department;
					worksheet.Cells["E" + i].Value = registration.NoOfAdults;
					worksheet.Cells["F" + i].Value = registration.NoOfChildren;
					worksheet.Cells["G" + i].Value = registration.BringsTrailer;

					i++;
				}
				var excelData = package.GetAsByteArray();
				var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				var fileName = "MyWorkbook.xlsx";

				return File(excelData, contentType, fileName);
			}
		}
	}
}
