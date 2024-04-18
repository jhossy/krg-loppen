using Krg.Domain;
using System.Collections.ObjectModel;
using Umbraco.Cms.Core.Models;

namespace Krg.Services
{
	public class RegistrationViewModel
	{
        public RegistrationViewModel(IContent eventNode, List<Registration> registrations)
        {
            Content = eventNode;
            Registrations = new ReadOnlyCollection<Registration>(registrations);
        }

		public IContent Content { get; }

		public ReadOnlyCollection<Registration> Registrations { get; }		
    }
}
