using Krg.Domain;
using Krg.Services.Models;

namespace Krg.Services.Interfaces
{
	public interface IExcelService
    {
        byte[] CreateExcel(int year, List<BackofficeRegistrationDto> registrations);
        
        byte[] CreateGroupedExcel(int year, List<BackofficeRegistrationDto> registrations);
    }
}
