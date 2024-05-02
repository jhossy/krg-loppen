using Krg.Domain;
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
				worksheet.Cells[1, 2].Value = "Navn";
				worksheet.Cells[1, 3].Value = "Email";
				worksheet.Cells[1, 4].Value = "Afdeling";
				worksheet.Cells[1, 5].Value = "Antal voksne";
				worksheet.Cells[1, 6].Value = "Antal børn";
				worksheet.Cells[1, 7].Value = "Medbringer trailer";
				worksheet.Cells[1, 8].Value = "Annulleret";

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
					worksheet.Cells["H" + i].Value = registration.IsCancelled;

					i++;
				}
				var excelData = package.GetAsByteArray();

				return excelData;
			}
		}
	}
}
