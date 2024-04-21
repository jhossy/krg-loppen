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

	//  public HomePageViewModel(IPublishedContent? publishedContent, IEnumerable<IGrouping<int, RegistrationViewModel>> events)
	//  {
	//CurrentPage = publishedContent;
	//Events = events;
	//  }


    public List<RegistrationViewModel> Events { get; set; }
}
