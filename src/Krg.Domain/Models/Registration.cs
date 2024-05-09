using Krg.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Krg.Domain.Models
{
    public class Registration
    {
        public Registration(EventRegistration eventRegistration)
        {
            Id = eventRegistration.Id;
			IsCancelled = eventRegistration.IsCancelled;
			BringsTrailer = eventRegistration.BringsTrailer;
            Department = eventRegistration.Department;
            Email = eventRegistration.Email;
            EventDate = eventRegistration.EventDate;            
            Name = eventRegistration.Name;
            NoOfAdults = eventRegistration.NoOfAdults;
            NoOfChildren = eventRegistration.NoOfChildren;
            PhoneNo = eventRegistration.PhoneNo;
            ShowName = eventRegistration.ShowName;
            UpdateTimeUtc = eventRegistration.UpdateTimeUtc;
        }

		public int Id { get; }

		public DateTime EventDate { get; }

		public DateTime UpdateTimeUtc { get; }

		public string Name { get; } = null!;

        public string Department { get; } = null!;

        public int NoOfAdults { get; }

        public int NoOfChildren { get; }

        public string PhoneNo { get; } = null!;

        [EmailAddress]
        public string Email { get; } = null!;

        public bool BringsTrailer { get; }

        public bool ShowName { get; }

        public bool IsCancelled { get; }
    }
}
