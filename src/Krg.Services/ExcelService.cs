using Krg.Domain;
using OfficeOpenXml;
using System.Globalization;

namespace Krg.Web.Controllers
{
	public class ExcelService : IExcelService
	{
		public byte[] CreateExcel(List<Registration> registrations)
		{
			//using (var package = new ExcelPackage(new FileInfo($"Export_{DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("da-DK"))}.xlsx")))
			//{
			//	int year = registrations.First().EventDate.Year;
			//	var worksheet = package.Workbook.Worksheets.Add(year.ToString());

			//	//Add the headers
			//	worksheet.Cells[1, 1].Value = "Dato";
			//	worksheet.Cells[1, 2].Value = "Navn";
			//	worksheet.Cells[1, 3].Value = "Email";
			//	worksheet.Cells[1, 4].Value = "Afdeling";
			//	worksheet.Cells[1, 5].Value = "Antal voksne";
			//	worksheet.Cells[1, 6].Value = "Antal børn";
			//	worksheet.Cells[1, 7].Value = "Medbringer trailer";

			//	var excelData = package.GetAsByteArray();
			//	var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			//	var fileName = "MyWorkbook.xlsx";

			//}
			throw new NotImplementedException();
		}
	}
}
