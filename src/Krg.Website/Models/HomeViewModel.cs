namespace Krg.Website.Models;

public class HomeViewModel
{
	public HomeViewModel()
	{
		Events = new List<RegistrationViewModel>();
	}

	public List<RegistrationViewModel> Events { get; set; }
}