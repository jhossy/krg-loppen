using Krg.Domain.Models;
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

        public bool IsFullyBooked => Registrations.Count() > 4;

		public int TotalNoOfParticipants => TotalNoOfAdults + TotalNoOfChildren;

		public int TotalNoOfAdults => Registrations.Sum(x => x.NoOfAdults);

		public int TotalNoOfChildren => Registrations.Sum(x => x.NoOfChildren);

		public int TotalNoOfTrailers => Registrations.Count(x => x.BringsTrailer);

		public ReadOnlyCollection<Registration> Registrations { get; }		
    }
}
