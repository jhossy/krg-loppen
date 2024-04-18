using Krg.Database;
using Krg.Database.Models;
using Krg.Domain;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace Krg.Services
{
	public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IContentService _contentService;

		public EventRegistrationService(
			IRegistrationRepository registrationRepository,
			IContentService contentService)
		{
			_registrationRepository = registrationRepository;
			_contentService = contentService;
		}

		public void AddRegistration(int umbracoNodeId, AddRegistrationRequest addRegistrationRequest)
		{
			_registrationRepository.AddRegistration(new EventRegistration
			{
				BringsTrailer = addRegistrationRequest.BringsTrailer,
				Department = addRegistrationRequest.Department,
				Email = addRegistrationRequest.Email,
				EventDate = addRegistrationRequest.EventDate,
				Name = addRegistrationRequest.Name,
				NoOfAdults = addRegistrationRequest.NoOfAdults,
				NoOfChildren = addRegistrationRequest.NoOfChildren,
				PhoneNo = addRegistrationRequest.PhoneNo,
				ShowName = addRegistrationRequest.ShowName,
				UmbracoEventNodeId = umbracoNodeId,
				UpdateTimeUtc = DateTime.UtcNow
			});
		}

		public List<RegistrationViewModel> GetRegistrations()
		{
			var dbRegistrations = _registrationRepository
				.GetRegistrations()
				.ToList();

			var rootNode = _contentService.GetRootContent().FirstOrDefault();
			
			var registrationRoot = Constants.RegistrationRootId; //Todo read from config
			
			var umbracoRegistrations = _contentService
				.GetPagedDescendants(registrationRoot, 0, 100, out long totalRecords)
				.Where(x => x.ContentTypeId == Constants.EventTypeId); //Todo select by node alias

			List<RegistrationViewModel> results = new List<RegistrationViewModel>();

			foreach (IContent umbRegistration in umbracoRegistrations)
			{
				var filteredRegistrationsByDate = dbRegistrations.Where(x => x.EventDate == umbRegistration.GetValue<DateTime>("date"))
					.Select(reg => new Registration(reg))
					.ToList();

				var registrationDto = new RegistrationViewModel(umbRegistration, filteredRegistrationsByDate);

				results.Add(registrationDto);
			}
			return results;
		}

		//public void RemoveRegistration(Registration eventRegistration)
		//{
		//	using var scope = _scopeProvider.CreateScope();

		//	scope.Database.Delete<EventRegistration>($"DELETE * FROM EventRegistration WHERE Email = {eventRegistration.Email} && EventDate = {eventRegistration.EventDate}");

		//	scope.Complete();
		//}
	}
}

//public static class GlobalSettingsExtensions
//{
//	public static IPublishedContent GlobalSettings(this UmbracoHelper umbraco)
//	{

//	}
//}
