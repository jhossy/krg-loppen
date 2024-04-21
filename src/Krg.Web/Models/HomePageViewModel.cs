using Krg.Services;
using Umbraco.Cms.Core.Models.PublishedContent;

public class HomePageViewModel : PublishedContentWrapped
{
	public HomePageViewModel(
		IPublishedContent content, 
		IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
	{
		Events = new List<RegistrationViewModel>();
	}

    public List<RegistrationViewModel> Events { get; set; }
}
