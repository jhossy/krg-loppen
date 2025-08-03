using System.Collections.ObjectModel;
using Krg.Domain.Models;

namespace Krg.Website.Models
{
    public class RegistrationViewModel
	{
        public RegistrationViewModel(EventDate evenDate, List<Registration> registrations)
        {
			EventContent = evenDate;
            Registrations = new ReadOnlyCollection<Registration>(registrations);
			ContactName = EventContent.ContactName ?? string.Empty;
			ContactPhone = EventContent.ContactPhone ?? string.Empty;
			ContactEmail = EventContent.ContactEmail ?? string.Empty;
			EventNote = !string.IsNullOrEmpty(EventContent.Note) ? EventContent.Note.Substring(0, Math.Min(100, EventContent.Note.Length)) : string.Empty;
		}

		public EventDate EventContent { get; }

        public bool IsFullyBooked => Registrations.Count() > 4;

		public int TotalNoOfParticipants => TotalNoOfAdults + TotalNoOfChildren;

		public int TotalNoOfAdults => Registrations.Sum(x => x.NoOfAdults);

		public int TotalNoOfChildren => Registrations.Sum(x => x.NoOfChildren);

		public int TotalNoOfTrailers => Registrations.Count(x => x.BringsTrailer);

		public string ContactName { get; }

		public string ContactPhone { get; }

		public string ContactEmail { get; }
		
		public string EventNote { get; }

		public ReadOnlyCollection<Registration> Registrations { get; }		
    }
}
