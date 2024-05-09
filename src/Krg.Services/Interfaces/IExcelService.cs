using Krg.Domain;

namespace Krg.Services.Interfaces
{
	public interface IExcelService
    {
        byte[] CreateExcel(int year, List<BackofficeRegistrationDto> registrations);
    }
}
