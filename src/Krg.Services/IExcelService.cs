using Krg.Domain;
using Krg.Domain.Models;

namespace Krg.Web.Controllers
{
    public interface IExcelService
	{
		byte[] CreateExcel(int year, List<BackofficeRegistrationDto> registrations);
	}
}
