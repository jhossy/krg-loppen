using Krg.Domain;
using Krg.Domain.Models;

namespace Krg.Services.Interfaces
{
    public interface IExcelService
    {
        byte[] CreateExcel(int year, List<BackofficeRegistrationDto> registrations);
    }
}
