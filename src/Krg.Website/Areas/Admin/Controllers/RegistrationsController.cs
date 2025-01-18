using Krg.Domain;
using Krg.Services.Interfaces;
using Krg.Services.Models;
using Krg.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class RegistrationsController(
    IEventRegistrationService eventRegistrationService,
    IEmailNotificationService emailNotificationService,
    IExcelService excelService)
    : BaseAdminController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    // [HttpPost]
    // public List<GroupedRegistration> GetGroupedRegistrations([FromBody]GetRequestDto getRequest)
    // {
    //     int parsedYear = getRequest.Year == 0 ? DateTime.Now.Year : getRequest.Year;
    //
    //     var regs = eventRegistrationService
    //         .GetNonDeletedRegistrations(parsedYear)
    //         .OrderBy(x => x.Department)
    //         .ThenBy(reg => reg.Name)
    //         .Select(reg => new BackofficeRegistrationDto(reg))
    //         .ToList();
    //
    //     return regs.GroupBy(reg => new { reg.Department })
    //         .Select(group =>
    //             new GroupedRegistration
    //             {
    //                 Department = group.Key.Department,
    //                 Registrations = group.GroupBy(g => g.Name).ToDictionary(g => g.Key, g => g.ToList())
    //             })
    //         .ToList();
    // }
    
    // [HttpPost]
    // public List<BackofficeRegistrationDto> GetRegistrations([FromBody]GetRequestDto getRequest)
    // {
    //     int parsedYear = getRequest.Year == 0 ? DateTime.Now.Year : getRequest.Year;
    //
    //     var regs = eventRegistrationService
    //         .GetNonDeletedRegistrations(parsedYear)
    //         .OrderBy(x => x.Department)
    //         .ThenBy(reg => reg.Name)
    //         .Select(reg => new BackofficeRegistrationDto(reg))
    //         .ToList();
    //
    //     var tmp = regs.GroupBy(reg => new{ reg.Department, reg.Name })
    //         .Select(group => new { Department = group.Key.Department, Name = group.Key.Name, Events = group});
    //
    //     var tmp2 = GetGroupedRegistrations(getRequest);
    //     
    //     return regs;
    // }
    
    [HttpPost]
    public List<BackofficeRegistrationDto> GetRegistrations([FromBody]GetRequestDto getRequest)
    {
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
    [ValidateAntiForgeryToken]
    public IActionResult ExportAsGroupedExcel(int year)
    {
        List<BackofficeRegistrationDto> registrations = GetAllRegistrations(year);
        
        var grouped = registrations.OrderBy(x => x.Department)
                                                        .ThenBy(x => x.Name)
                                                        // .Select(group =>
                                                        //     new GroupedRegistration
                                                        //     {
                                                        //         Department = group.Key.Department,
                                                        //         Registrations = group.GroupBy(g => g.Name).ToDictionary(g => g.Key, g => g.ToList())
                                                        //     })
                                                        .ToList();

        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			
        string fileName = $"Export_{DateTime.Now.ToDkExportDate()}.xlsx";

        byte[] excelData = excelService.CreateGroupedExcel(year, grouped);

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