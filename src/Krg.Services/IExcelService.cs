using Krg.Domain;

namespace Krg.Web.Controllers
{
	public interface IExcelService
	{
		byte[] CreateExcel(List<Registration> registrations);
	}
}
