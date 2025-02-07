using Krg.Database.Models;
using Krg.Services.Interfaces;
using Krg.Services.Models;
using Krg.Web.Extensions;
using Krg.Website.Areas.Admin.Models;
using Krg.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class RegistrationsController(
    IEventRegistrationService eventRegistrationService,
    IEmailNotificationService emailNotificationService,
    IExcelService excelService)
    : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public List<BackofficeRegistrationDto> GetRegistrations([FromBody]GetRequestDto getRequest)
    {
        DateRange dateRange = new DateRange(getRequest.StartDate, getRequest.EndDate);
    
        return eventRegistrationService
            .GetNonDeletedRegistrations(dateRange)
            .OrderBy(reg => reg.EventDate)
            .ThenBy(reg => reg.UpdateTimeUtc)
            .Select(reg => new BackofficeRegistrationDto(reg))
            .ToList();
    }
    
    private List<BackofficeRegistrationDto> GetAllRegistrations(DateRange dateRange)
    {
        return eventRegistrationService
            .GetAllRegistrations(dateRange)
            .OrderBy(reg => reg.EventDate)
            .ThenBy(reg => reg.UpdateTimeUtc)
            .Select(reg => new BackofficeRegistrationDto(reg))
            .ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExportAsExcel(string submitExport, DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
        {
            ModelState.AddModelError("endDate", Translations.Registrations.EndDateValidationMessage);
            
            return View("Index");
        }
        
        return submitExport == Translations.Registrations.ButtonExportText ? ExportExcel(startDate, endDate) : ExportAsGroupedExcel(startDate, endDate);
    }

    private IActionResult ExportExcel(DateOnly startDate, DateOnly endDate)
    {
        var dateRange = new DateRange(startDate, endDate);
        
        List<BackofficeRegistrationDto> registrations = GetAllRegistrations(dateRange);

        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			
        string fileName = $"Export_{DateTime.Now.ToDkExportDate()}.xlsx";

        byte[] excelData = excelService.CreateExcel(dateRange.StartDate.Year, registrations);

        return File(excelData, contentType, fileName);
    }

    private IActionResult ExportAsGroupedExcel(DateOnly startDate, DateOnly endDate)
    {
        var dateRange = new DateRange(startDate, endDate);
        
        List<BackofficeRegistrationDto> registrations = GetRegistrations(new GetRequestDto { StartDate = startDate, EndDate = endDate });
        
        var grouped = registrations
                                                        .OrderBy(x => x.Department)
                                                        .ThenBy(x => x.Name)
                                                        .ToList();

        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			
        string fileName = $"Export_Grouped_{DateTime.Now.ToDkExportDate()}.xlsx";

        byte[] excelData = excelService.CreateGroupedExcel(dateRange.StartDate.Year, grouped);

        return File(excelData, contentType, fileName);
    }

    [HttpPost]
    public IActionResult Delete([FromBody]DeleteRequestDto deleteRequestDto)
    {
        var registration = eventRegistrationService.GetById(deleteRequestDto.Id);

        if (registration == null) return Ok();

        eventRegistrationService.RemoveRegistration(deleteRequestDto.Id);

        emailNotificationService.CancelReminder(deleteRequestDto.Id);

        return Ok(GetRegistrations(new GetRequestDto{StartDate = deleteRequestDto.StartDate, EndDate = deleteRequestDto.EndDate}));
    }
}