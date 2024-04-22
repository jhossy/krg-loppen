using Krg.Domain;
using System.Collections.ObjectModel;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace Krg.Services
{
	public class RegistrationViewModel
	{
        public RegistrationViewModel(Event eventNode, List<Registration> registrations)
        {
            Content = eventNode;
            Registrations = new ReadOnlyCollection<Registration>(registrations);
        }

		public Event Content { get; }

        public bool IsFullyBooked { get; } = true;

		public ReadOnlyCollection<Registration> Registrations { get; }		
    }
}
