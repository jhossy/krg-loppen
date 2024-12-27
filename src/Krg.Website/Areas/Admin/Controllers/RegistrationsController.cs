using Krg.Domain;
using Krg.Services.Interfaces;
using Krg.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class RegistrationsController(
    IEventRegistrationService eventRegistrationService,
    IEmailNotificationService emailNotificationService,
    IExcelService excelService)
    : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public List<BackofficeRegistrationDto> GetRegistrations([FromBody]GetRequestDto getRequest)
    {
        Thread.Sleep(3000);
        int parsedYear = getRequest.Year == 0 ? DateTime.Now.Year : getRequest.Year;

        return eventRegistrationService
            .GetNonDeletedRegistrations(parsedYear)
            .OrderBy(reg => reg.EventDate)
            .ThenBy(reg => reg.UpdateTimeUtc)
            .Select(reg => new BackofficeRegistrationDto(reg))
            .ToList();
    }

    private List<BackofficeRegistrationDto> GetAllRegistrations(int year)
    {
        int parsedYear = year == 0 ? DateTime.Now.Year : year;

        return eventRegistrationService
            .GetAllRegistrations(parsedYear)
            .OrderBy(reg => reg.EventDate)
            .ThenBy(reg => reg.UpdateTimeUtc)
            .Select(reg => new BackofficeRegistrationDto(reg))
            .ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExportAsExcel(int year)
    {
        List<BackofficeRegistrationDto> registrations = GetAllRegistrations(year);

        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			
        string fileName = $"Export_{DateTime.Now.ToDkExportDate()}.xlsx";

        byte[] excelData = excelService.CreateExcel(year, registrations);

        return File(excelData, contentType, fileName);
    }

    [HttpPost]
    public IActionResult Delete([FromBody]DeleteRequestDto deleteRequestDto)
    {
        var registration = eventRegistrationService.GetById(deleteRequestDto.Id);

        if (registration == null) return Ok();

        eventRegistrationService.RemoveRegistration(deleteRequestDto.Id);

        emailNotificationService.CancelReminder(deleteRequestDto.Id);

        return Ok(GetRegistrations(new GetRequestDto{Year = deleteRequestDto.Year}));
    }
}

public class GetRequestDto
{
    public int Year { get; set; }
}

public class DeleteRequestDto
{
    public int Id { get; set; }
    
    public int Year { get; set; }
}