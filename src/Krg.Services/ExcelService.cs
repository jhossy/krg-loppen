using Krg.Domain;
using Krg.Services.Interfaces;
using OfficeOpenXml;
using System.Globalization;

namespace Krg.Web.Controllers
{
    public class ExcelService : IExcelService
	{
		private string ToDkDateFormat(DateTime dateTime) => dateTime.ToString("dd-MM-yyyy", new CultureInfo("da-DK"));

		public byte[] CreateExcel(int year, List<BackofficeRegistrationDto> registrations)
		{
			using (var package = new ExcelPackage(new FileInfo($"Export_{ToDkDateFormat(DateTime.Now)}.xlsx")))
			{				
				var worksheet = package.Workbook.Worksheets.Add(year.ToString());

				//Add the headers
				worksheet.Cells[1, 1].Value = "Dato";
				worksheet.Cells[1, 2].Value = "Senest opdateret";
				worksheet.Cells[1, 3].Value = "Navn";
				worksheet.Cells[1, 4].Value = "Email";
				worksheet.Cells[1, 5].Value = "Afdeling";
				worksheet.Cells[1, 6].Value = "Antal voksne";
				worksheet.Cells[1, 7].Value = "Antal børn";
				worksheet.Cells[1, 8].Value = "Medbringer trailer";
				worksheet.Cells[1, 9].Value = "Annulleret";

				//Add some items...
				int i = 2;
				foreach (var registration in registrations)
				{
					worksheet.Cells["A" + i].Value = registration.EventDate;
					worksheet.Cells["B" + i].Value = registration.UpdateDate;
					worksheet.Cells["C" + i].Value = registration.Name;
					worksheet.Cells["D" + i].Value = registration.Email;
					worksheet.Cells["E" + i].Value = registration.Department;
					worksheet.Cells["F" + i].Value = registration.NoOfAdults;
					worksheet.Cells["G" + i].Value = registration.NoOfChildren;
					worksheet.Cells["H" + i].Value = registration.BringsTrailer;
					worksheet.Cells["I" + i].Value = registration.IsCancelled;

					i++;
				}
				var excelData = package.GetAsByteArray();

				return excelData;
			}
		}
	}
}
