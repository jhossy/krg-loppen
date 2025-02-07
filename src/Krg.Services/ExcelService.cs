using System.Globalization;
using Krg.Services.Interfaces;
using Krg.Services.Models;
using OfficeOpenXml;

namespace Krg.Services
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

		public byte[] CreateGroupedExcel(int year, List<BackofficeRegistrationDto> registrations)
		{
			using (var package = new ExcelPackage(new FileInfo($"Export_Grouped_{ToDkDateFormat(DateTime.Now)}.xlsx")))
			{				
				var worksheet = package.Workbook.Worksheets.Add(year.ToString());

				//Add the headers
				worksheet.Cells[1, 1].Value = "Afdeling";
				worksheet.Cells[1, 2].Value = "Navn";
				worksheet.Cells[1, 3].Value = "Dato#1";
				worksheet.Cells[1, 4].Value = "Dato#2";
				worksheet.Cells[1, 5].Value = "Dato#3";
				worksheet.Cells[1, 6].Value = "Dato#4";
				worksheet.Cells[1, 7].Value = "Dato#5";

				//Add some items...
				int lineCounter = 2;
				foreach (var depGroup in registrations.GroupBy(x => x.Department.Trim(), StringComparer.InvariantCultureIgnoreCase))
				{
					worksheet.Cells["A" + lineCounter].Value = depGroup.Key; //department
					
					foreach (var nameGroup in depGroup.GroupBy(g => g.Name.Trim(), StringComparer.InvariantCultureIgnoreCase))
					{
						worksheet.Cells["B" + lineCounter].Value = nameGroup.Key; //name
						
						string[] cols = new string[] {"C", "D", "E", "F", "G", "H", "I", "J", "K"};
						int k = 0;
						foreach (var regPerson in nameGroup.OrderBy(x => DateTime.Parse(x.EventDate)))
						{
							string col = cols[k];
							worksheet.Cells[col + lineCounter].Value = regPerson.EventDate;
							k++;	
						}
						
						lineCounter++;
					}
				}
				var excelData = package.GetAsByteArray();

				return excelData;
			}
		}
	}
}
